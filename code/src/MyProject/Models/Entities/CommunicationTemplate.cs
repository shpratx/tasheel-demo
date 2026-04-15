using System.ComponentModel.DataAnnotations;

namespace MyProject.Models.Entities;

public class CommunicationTemplate
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string TemplateCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string TemplateName { get; set; } = string.Empty;

    [Required]
    public CommunicationChannel Channel { get; set; }

    [MaxLength(200)]
    public string? Subject { get; set; }

    [Required]
    public string Body { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public bool ComplianceApproved { get; set; }

    public DateTime? ApprovedDate { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime UpdatedDate { get; set; }
}

public enum CommunicationChannel
{
    Email,
    SMS,
    Push,
    InApp
}