namespace MyProject.Models.Dtos;

public class CommunicationTemplate
{
    public Guid Id { get; set; }
    public string TemplateCode { get; set; } = string.Empty;
    public string TemplateName { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string Body { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool ComplianceApproved { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}