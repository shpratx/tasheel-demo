using Microsoft.Extensions.Options;
using MyProject.Configuration;
using MyProject.DataStores.Interfaces;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Services.Implementations;

public class EligibilityService : IEligibilityService
{
    private readonly ICustomerDataStore _customerDataStore;
    private readonly ILogger<EligibilityService> _logger;
    private readonly BridgeNowSettings _settings;

    public EligibilityService(
        ICustomerDataStore customerDataStore,
        ILogger<EligibilityService> logger,
        IConfiguration configuration)
    {
        _customerDataStore = customerDataStore;
        _logger = logger;
        _settings = configuration.GetSection("BridgeNowSettings").Get<BridgeNowSettings>() ?? new BridgeNowSettings();
    }

    public async Task<EligibilityResponse> CheckEligibilityAsync(EligibilityRequest request)
    {
        _logger.LogInformation("Checking eligibility for customer {CustomerId}", request.CustomerId);

        // Fetch customer data
        var customer = await _customerDataStore.GetByIdAsync(request.CustomerId);
        if (customer == null)
        {
            throw new Exceptions.NotFoundException("Customer", request.CustomerId);
        }

        var ruleResults = new List<RuleResult>();

        // Rule 1: Credit Score Check
        var creditScore = request.CreditScore ?? customer.CreditScore;
        var creditScoreRule = new RuleResult
        {
            RuleName = "CreditScoreCheck",
            Passed = creditScore >= 300,
            Message = creditScore >= 300 ? "Credit score is acceptable" : "Credit score is below minimum threshold"
        };
        ruleResults.Add(creditScoreRule);

        // Rule 2: Annual Income Check
        var annualIncome = request.AnnualIncome ?? 0;
        var incomeRule = new RuleResult
        {
            RuleName = "AnnualIncomeCheck",
            Passed = annualIncome > 0,
            Message = annualIncome > 0 ? "Annual income is acceptable" : "Annual income must be positive"
        };
        ruleResults.Add(incomeRule);

        // Rule 3: Loan Amount Check
        if (request.LoanAmount.HasValue)
        {
            var loanAmountRule = new RuleResult
            {
                RuleName = "LoanAmountCheck",
                Passed = request.LoanAmount >= 10000 && request.LoanAmount <= 500000,
                Message = (request.LoanAmount >= 10000 && request.LoanAmount <= 500000) 
                    ? "Loan amount is within acceptable range" 
                    : "Loan amount must be between $10,000 and $500,000"
            };
            ruleResults.Add(loanAmountRule);
        }

        // Rule 4: Employment Status Check
        var employmentStatusRule = new RuleResult
        {
            RuleName = "EmploymentStatusCheck",
            Passed = !string.IsNullOrEmpty(request.EmploymentStatus) && 
                     (request.EmploymentStatus == "Employed" || request.EmploymentStatus == "SelfEmployed"),
            Message = (!string.IsNullOrEmpty(request.EmploymentStatus) && 
                      (request.EmploymentStatus == "Employed" || request.EmploymentStatus == "SelfEmployed"))
                ? "Employment status is acceptable"
                : "Must be employed or self-employed"
        };
        ruleResults.Add(employmentStatusRule);

        // Determine overall eligibility
        var isEligible = ruleResults.All(r => r.Passed);

        // Determine STP eligibility (stricter criteria)
        var stpSettings = _settings.StpEligibility ?? new StpEligibilitySettings();
        var isStpEligible = isEligible &&
                           creditScore >= stpSettings.MinCreditScore &&
                           customer.DebtToIncomeRatio <= stpSettings.MaxDebtToIncomeRatio &&
                           annualIncome >= stpSettings.MinAnnualIncome;

        _logger.LogInformation("Eligibility check completed. IsEligible: {IsEligible}, IsStpEligible: {IsStpEligible}", 
            isEligible, isStpEligible);

        return new EligibilityResponse
        {
            IsEligible = isEligible,
            IsStpEligible = isStpEligible,
            Status = isStpEligible ? "STP_ELIGIBLE" : (isEligible ? "MANUAL_REVIEW_REQUIRED" : "NOT_ELIGIBLE"),
            RuleResults = ruleResults,
            EvaluatedDate = DateTime.UtcNow
        };
    }
}