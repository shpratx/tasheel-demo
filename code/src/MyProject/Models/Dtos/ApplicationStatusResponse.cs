namespace MyProject.Models.Dtos;

public class ApplicationStatusResponse
{
    public Guid ApplicationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
    public bool IsStpProcessed { get; set; }
    public string? CurrentStage { get; set; }
}