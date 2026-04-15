namespace MyProject.Models.Entities;

public class CommunicationTemplateEntity
{
    public Guid Id { get; set; }
    public string TemplateCode { get; set; } = string.Empty;
    public string TemplateName { get; set; } = string.Empty;
    public CommunicationChannel Channel { get; set; }
    public string? Subject { get; set; }
    public string Body { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool ComplianceApproved { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public enum CommunicationChannel
{
    Email,
    SMS,
    Push,
    InApp
}