using MyProject.DataStores.Interfaces;
using MyProject.Exceptions;
using MyProject.Models.Entities;
using MyProject.Services.Interfaces;

namespace MyProject.Services.Implementations;

public class WorkflowService : IWorkflowService
{
    private readonly IApplicationDataStore _applicationDataStore;
    private readonly ILogger<WorkflowService> _logger;

    public WorkflowService(
        IApplicationDataStore applicationDataStore,
        ILogger<WorkflowService> logger)
    {
        _applicationDataStore = applicationDataStore;
        _logger = logger;
    }

    public async Task TriggerStpWorkflowAsync(Guid applicationId)
    {
        _logger.LogInformation("Triggering STP workflow for application {ApplicationId}", applicationId);

        var application = await _applicationDataStore.GetByIdAsync(applicationId);
        if (application == null)
        {
            throw new NotFoundException($"Application {applicationId} not found");
        }

        try
        {
            // Update status to processing
            application.Status = ApplicationStatus.Processing;
            application.CurrentStage = "STP_WORKFLOW";
            await _applicationDataStore.UpdateAsync(application);

            // Simulate automated approval (in real scenario, this would involve more checks)
            await Task.Delay(100); // Simulate processing time

            // Auto-approve
            application.Status = ApplicationStatus.Approved;
            application.ApprovedDate = DateTime.UtcNow;
            application.CurrentStage = "APPROVED";
            await _applicationDataStore.UpdateAsync(application);

            _logger.LogInformation("STP workflow completed successfully for application {ApplicationId}", applicationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "STP workflow failed for application {ApplicationId}", applicationId);
            throw new WorkflowException($"STP workflow failed for application {applicationId}", ex);
        }
    }

    public async Task TriggerManualReviewWorkflowAsync(Guid applicationId)
    {
        _logger.LogInformation("Triggering manual review workflow for application {ApplicationId}", applicationId);

        var application = await _applicationDataStore.GetByIdAsync(applicationId);
        if (application == null)
        {
            throw new NotFoundException($"Application {applicationId} not found");
        }

        try
        {
            // Update status to under review
            application.Status = ApplicationStatus.UnderReview;
            application.CurrentStage = "MANUAL_REVIEW";
            application.ReviewAssignedDate = DateTime.UtcNow;
            await _applicationDataStore.UpdateAsync(application);

            _logger.LogInformation("Manual review workflow initiated for application {ApplicationId}", applicationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Manual review workflow failed for application {ApplicationId}", applicationId);
            throw new WorkflowException($"Manual review workflow failed for application {applicationId}", ex);
        }
    }
}