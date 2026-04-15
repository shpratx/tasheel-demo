namespace MyProject.Models.Entities;

public class LandingPageContent
{
    public Guid Id { get; set; }
    public string PageTitle { get; set; } = string.Empty;
    public string HeroText { get; set; } = string.Empty;
    public string CtaText { get; set; } = string.Empty;
    public string? BrandingAssetUrl { get; set; }
    public string? ProductHighlights { get; set; }
    public string? EligibilityCriteria { get; set; }
    public bool IsActive { get; set; }
    public int Version { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}