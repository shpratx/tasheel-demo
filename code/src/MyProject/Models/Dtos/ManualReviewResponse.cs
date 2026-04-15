namespace MyProject.Models.Dtos;

public class ManualReviewResponse
{
    public Guid ApplicationId { get; set; }
    public string ReviewStatus { get; set; } = string.Empty;
    public string? AssignedReviewer { get; set; }
    public DateTime AssignedDate { get; set; }
}