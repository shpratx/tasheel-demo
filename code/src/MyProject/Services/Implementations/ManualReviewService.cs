using MyProject.DataStores.Interfaces;
using MyProject.Exceptions;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Services.Implementations;

public class ManualReviewService : IManualReviewService
{
    private readonly IApplicationDataStore _applicationDataStore;
    private readonly ILogger<ManualReviewService> _logger;

    public ManualReviewService(
        IApplicationDataStore applicationDataStore,
        ILogger<ManualReviewService> logger)
    {
        _applicationDataStore = applicationDataStore;
        _logger = logger;
    }

    public async Task<ManualReviewResponse> TriggerManualReviewAsync(Guid applicationId, ManualReviewRequest request)
    {
        _logger.LogInformation("Triggering manual review for application {ApplicationId}", applicationId);

        var application = await _applicationDataStore.GetByIdAsync(applicationId);
        if (application == null)
        {
            throw new NotFoundException("Application", applicationId);
        }

        // Update application to manual review status
        application.Status = Models.Entities.ApplicationStatus.UnderReview;
        application.CurrentStage = "MANUAL_REVIEW";
        application.ReviewAssignedDate = DateTime.UtcNow;
        await _applicationDataStore.UpdateAsync(application);

        return new ManualReviewResponse
        {
            ApplicationId = applicationId,
            ReviewStatus = "ASSIGNED",
            AssignedReviewer = "ReviewTeam",
            AssignedDate = DateTime.UtcNow
        };
    }
}