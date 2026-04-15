using System.ComponentModel.DataAnnotations;

namespace MyProject.Models.Entities;

public class LandingPageContent
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string PageTitle { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string HeroText { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CtaText { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? BrandingAssetUrl { get; set; }

    public string? ProductHighlights { get; set; }

    public string? EligibilityCriteria { get; set; }

    public bool IsActive { get; set; } = true;

    public int Version { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime UpdatedDate { get; set; }
}