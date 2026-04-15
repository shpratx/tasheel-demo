using MyProject.DataStores.Interfaces;
using MyProject.Exceptions;
using MyProject.Models.Dtos;
using MyProject.Models.Entities;
using MyProject.Services.Interfaces;

namespace MyProject.Services.Implementations;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationDataStore _applicationDataStore;
    private readonly ICustomerDataStore _customerDataStore;
    private readonly IEligibilityService _eligibilityService;
    private readonly IWorkflowService _workflowService;
    private readonly ILogger<ApplicationService> _logger;

    public ApplicationService(
        IApplicationDataStore applicationDataStore,
        ICustomerDataStore customerDataStore,
        IEligibilityService eligibilityService,
        IWorkflowService workflowService,
        ILogger<ApplicationService> logger)
    {
        _applicationDataStore = applicationDataStore;
        _customerDataStore = customerDataStore;
        _eligibilityService = eligibilityService;
        _workflowService = workflowService;
        _logger = logger;
    }

    public async Task<ApplicationResponse> SubmitApplicationAsync(ApplicationRequest request)
    {
        _logger.LogInformation("Starting application submission for customer {CustomerId}", request.CustomerId);

        try
        {
            // Validate customer exists or create mock customer for demo
            var customer = await _customerDataStore.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                // Create mock customer for demo purposes
                customer = new Customer
                {
                    Id = request.CustomerId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    DateOfBirth = DateTime.UtcNow.AddYears(-30),
                    SSN = "000-00-0000",
                    CreditScore = request.CreditScore ?? 700,
                    DebtToIncomeRatio = 0.3m,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };
                await _customerDataStore.CreateAsync(customer);
            }

            // Check eligibility
            var eligibilityRequest = new EligibilityRequest
            {
                CustomerId = request.CustomerId,
                LoanAmount = request.LoanAmount,
                AnnualIncome = request.AnnualIncome,
                EmploymentStatus = request.EmploymentStatus,
                CreditScore = request.CreditScore,
                PropertyValue = request.PropertyValue
            };

            var eligibilityResult = await _eligibilityService.CheckEligibilityAsync(eligibilityRequest);

            // Map employment status
            var employmentStatus = Enum.TryParse<ApplicationEmploymentStatus>(request.EmploymentStatus, out var empStatus)
                ? empStatus
                : ApplicationEmploymentStatus.Employed;

            // Create application entity
            var application = new Application
            {
                CustomerId = request.CustomerId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                LoanAmount = request.LoanAmount,
                AnnualIncome = request.AnnualIncome,
                EmploymentStatus = employmentStatus,
                CreditScore = request.CreditScore,
                PropertyValue = request.PropertyValue,
                Status = ApplicationStatus.Submitted,
                IsStpEligible = eligibilityResult.IsStpEligible,
                EligibilityStatus = eligibilityResult.Status,
                CurrentStage = eligibilityResult.IsStpEligible ? "STP_WORKFLOW" : "MANUAL_REVIEW",
                SubmittedDate = DateTime.UtcNow
            };

            // Save application
            application = await _applicationDataStore.CreateAsync(application);

            // Route to appropriate workflow
            if (eligibilityResult.IsStpEligible)
            {
                await _workflowService.TriggerStpWorkflowAsync(application.Id);
            }
            else
            {
                await _workflowService.TriggerManualReviewWorkflowAsync(application.Id);
            }

            _logger.LogInformation("Application submitted successfully. ApplicationId: {ApplicationId}", application.Id);

            return MapToResponse(application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to submit application for customer {CustomerId}", request.CustomerId);
            throw;
        }
    }

    public async Task<ApplicationStatusResponse> GetApplicationStatusAsync(Guid applicationId)
    {
        var application = await _applicationDataStore.GetByIdAsync(applicationId);
        if (application == null)
        {
            throw new NotFoundException($"Application {applicationId} not found");
        }

        return new ApplicationStatusResponse
        {
            ApplicationId = application.Id,
            Status = application.Status.ToString(),
            LastUpdated = application.UpdatedDate,
            IsStpProcessed = application.IsStpEligible,
            CurrentStage = application.CurrentStage
        };
    }

    public async Task<ApplicationResponse> UpdateApplicationAsync(Guid applicationId, ApplicationUpdateRequest request)
    {
        var application = await _applicationDataStore.GetByIdAsync(applicationId);
        if (application == null)
        {
            throw new NotFoundException($"Application {applicationId} not found");
        }

        // Update fields if provided
        if (!string.IsNullOrEmpty(request.FirstName))
            application.FirstName = request.FirstName;
        if (!string.IsNullOrEmpty(request.LastName))
            application.LastName = request.LastName;
        if (!string.IsNullOrEmpty(request.Email))
            application.Email = request.Email;
        if (!string.IsNullOrEmpty(request.PhoneNumber))
            application.PhoneNumber = request.PhoneNumber;
        if (request.LoanAmount.HasValue)
            application.LoanAmount = request.LoanAmount.Value;
        if (request.AnnualIncome.HasValue)
            application.AnnualIncome = request.AnnualIncome.Value;
        if (!string.IsNullOrEmpty(request.EmploymentStatus))
        {
            if (Enum.TryParse<ApplicationEmploymentStatus>(request.EmploymentStatus, out var empStatus))
                application.EmploymentStatus = empStatus;
        }
        if (request.CreditScore.HasValue)
            application.CreditScore = request.CreditScore;
        if (request.PropertyValue.HasValue)
            application.PropertyValue = request.PropertyValue;

        application = await _applicationDataStore.UpdateAsync(application);
        return MapToResponse(application);
    }

    private ApplicationResponse MapToResponse(Application application)
    {
        return new ApplicationResponse
        {
            Id = application.Id,
            CustomerId = application.CustomerId,
            FirstName = application.FirstName,
            LastName = application.LastName,
            Email = application.Email,
            PhoneNumber = application.PhoneNumber,
            LoanAmount = application.LoanAmount,
            AnnualIncome = application.AnnualIncome,
            EmploymentStatus = application.EmploymentStatus.ToString(),
            CreditScore = application.CreditScore,
            PropertyValue = application.PropertyValue,
            Status = application.Status.ToString(),
            IsStpEligible = application.IsStpEligible,
            EligibilityStatus = application.EligibilityStatus,
            CurrentStage = application.CurrentStage,
            SubmittedDate = application.SubmittedDate,
            ApprovedDate = application.ApprovedDate,
            ReviewAssignedDate = application.ReviewAssignedDate,
            CreatedDate = application.CreatedDate,
            UpdatedDate = application.UpdatedDate
        };
    }
}