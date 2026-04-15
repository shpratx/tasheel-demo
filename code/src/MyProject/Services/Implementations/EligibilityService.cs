using MyProject.DataStores.Interfaces;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Services.Implementations;

public class EligibilityService : IEligibilityService
{
    private readonly ICustomerDataStore _customerDataStore;
    private readonly ILogger<EligibilityService> _logger;
    private readonly IConfiguration _configuration;

    public EligibilityService(
        ICustomerDataStore customerDataStore,
        ILogger<EligibilityService> logger,
        IConfiguration configuration)
    {
        _customerDataStore = customerDataStore;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<EligibilityResponse> CheckEligibilityAsync(EligibilityRequest request)
    {
        _logger.LogInformation("Checking eligibility for customer {CustomerId}", request.CustomerId);

        var ruleResults = new List<RuleResult>();

        // Get customer data
        var customer = await _customerDataStore.GetByIdAsync(request.CustomerId);

        // Get configuration values
        var minCreditScore = _configuration.GetValue<int>("BridgeNowSettings:StpEligibility:MinCreditScore", 700);
        var maxDebtToIncomeRatio = _configuration.GetValue<decimal>("BridgeNowSettings:StpEligibility:MaxDebtToIncomeRatio", 0.4m);
        var minAnnualIncome = _configuration.GetValue<decimal>("BridgeNowSettings:StpEligibility:MinAnnualIncome", 50000);

        // Rule 1: Credit Score Check
        var creditScore = request.CreditScore ?? customer?.CreditScore ?? 0;
        var creditScorePassed = creditScore >= minCreditScore;
        ruleResults.Add(new RuleResult
        {
            RuleName = "Credit Score Check",
            Passed = creditScorePassed,
            Message = creditScorePassed
                ? $"Credit score {creditScore} meets minimum requirement of {minCreditScore}"
                : $"Credit score {creditScore} is below minimum requirement of {minCreditScore}"
        });

        // Rule 2: Annual Income Check
        var annualIncome = request.AnnualIncome ?? 0;
        var incomePassed = annualIncome >= minAnnualIncome;
        ruleResults.Add(new RuleResult
        {
            RuleName = "Annual Income Check",
            Passed = incomePassed,
            Message = incomePassed
                ? $"Annual income ${annualIncome:N2} meets minimum requirement of ${minAnnualIncome:N2}"
                : $"Annual income ${annualIncome:N2} is below minimum requirement of ${minAnnualIncome:N2}"
        });

        // Rule 3: Debt-to-Income Ratio Check
        var debtToIncomeRatio = customer?.DebtToIncomeRatio ?? 0.3m;
        var dtiPassed = debtToIncomeRatio <= maxDebtToIncomeRatio;
        ruleResults.Add(new RuleResult
        {
            RuleName = "Debt-to-Income Ratio Check",
            Passed = dtiPassed,
            Message = dtiPassed
                ? $"Debt-to-income ratio {debtToIncomeRatio:P2} is within acceptable limit of {maxDebtToIncomeRatio:P2}"
                : $"Debt-to-income ratio {debtToIncomeRatio:P2} exceeds limit of {maxDebtToIncomeRatio:P2}"
        });

        // Rule 4: Employment Status Check
        var employmentStatus = request.EmploymentStatus ?? "Employed";
        var employmentPassed = employmentStatus == "Employed" || employmentStatus == "SelfEmployed";
        ruleResults.Add(new RuleResult
        {
            RuleName = "Employment Status Check",
            Passed = employmentPassed,
            Message = employmentPassed
                ? $"Employment status '{employmentStatus}' is acceptable"
                : $"Employment status '{employmentStatus}' does not meet requirements"
        });

        // Rule 5: Loan Amount Check
        var loanAmount = request.LoanAmount ?? 0;
        var loanAmountPassed = loanAmount >= 10000 && loanAmount <= 500000;
        ruleResults.Add(new RuleResult
        {
            RuleName = "Loan Amount Check",
            Passed = loanAmountPassed,
            Message = loanAmountPassed
                ? $"Loan amount ${loanAmount:N2} is within acceptable range"
                : $"Loan amount ${loanAmount:N2} is outside acceptable range ($10,000 - $500,000)"
        });

        // Determine overall eligibility
        var isEligible = ruleResults.All(r => r.Passed);

        // Determine STP eligibility (stricter criteria)
        var isStpEligible = isEligible && creditScore >= minCreditScore && debtToIncomeRatio <= maxDebtToIncomeRatio;

        var response = new EligibilityResponse
        {
            IsEligible = isEligible,
            IsStpEligible = isStpEligible,
            Status = isStpEligible ? "STP_ELIGIBLE" : (isEligible ? "MANUAL_REVIEW_REQUIRED" : "NOT_ELIGIBLE"),
            RuleResults = ruleResults,
            EvaluatedDate = DateTime.UtcNow
        };

        _logger.LogInformation("Eligibility check completed for customer {CustomerId}. Status: {Status}",
            request.CustomerId, response.Status);

        return response;
    }
}