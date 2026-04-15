using MyProject.Models.Dtos;

namespace MyProject.Services.Interfaces;

public interface IManualReviewService
{
    Task<ManualReviewResponse> TriggerManualReviewAsync(Guid applicationId, ManualReviewRequest request);
}