namespace MyProject.Services.Interfaces;

public interface IWorkflowService
{
    Task TriggerStpWorkflowAsync(Guid applicationId);
    Task TriggerManualReviewWorkflowAsync(Guid applicationId);
}