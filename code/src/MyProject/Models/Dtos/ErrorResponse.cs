namespace MyProject.Models.Dtos;

public class ErrorResponse
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string TraceId { get; set; } = string.Empty;
    public string ErrorCode { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public List<string> Details { get; set; } = new();
}