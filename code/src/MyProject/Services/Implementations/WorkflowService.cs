using MyProject.DataStores.Interfaces;
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
            _logger.LogWarning("Application {ApplicationId} not found for STP workflow", applicationId);
            return;
        }

        // Simulate STP processing delay
        await Task.Delay(TimeSpan.FromSeconds(2));

        // Update status to processing
        application.Status = ApplicationStatus.Processing;
        application.CurrentStage = "STP_WORKFLOW";
        await _applicationDataStore.UpdateAsync(application);

        // Simulate automated approval
        await Task.Delay(TimeSpan.FromSeconds(3));

        application.Status = ApplicationStatus.Approved;
        application.ApprovedDate = DateTime.UtcNow;
        application.CurrentStage = "APPROVED";
        await _applicationDataStore.UpdateAsync(application);

        _logger.LogInformation("STP workflow completed for application {ApplicationId}", applicationId);
    }

    public async Task TriggerManualReviewWorkflowAsync(Guid applicationId)
    {
        _logger.LogInformation("Triggering manual review workflow for application {ApplicationId}", applicationId);

        var application = await _applicationDataStore.GetByIdAsync(applicationId);
        if (application == null)
        {
            _logger.LogWarning("Application {ApplicationId} not found for manual review workflow", applicationId);
            return;
        }

        // Simulate delay before assigning to review queue
        await Task.Delay(TimeSpan.FromSeconds(1));

        application.Status = ApplicationStatus.UnderReview;
        application.CurrentStage = "MANUAL_REVIEW";
        application.ReviewAssignedDate = DateTime.UtcNow;
        await _applicationDataStore.UpdateAsync(application);

        _logger.LogInformation("Manual review workflow triggered for application {ApplicationId}", applicationId);
    }
}