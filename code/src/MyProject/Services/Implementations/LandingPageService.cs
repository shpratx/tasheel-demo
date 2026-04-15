using MyProject.DataStores.Interfaces;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Services.Implementations;

public class LandingPageService : ILandingPageService
{
    private readonly ILandingPageContentDataStore _contentDataStore;
    private readonly ILogger<LandingPageService> _logger;

    public LandingPageService(
        ILandingPageContentDataStore contentDataStore,
        ILogger<LandingPageService> logger)
    {
        _contentDataStore = contentDataStore;
        _logger = logger;
    }

    public async Task<LandingPageResponse> GetLandingPageContentAsync()
    {
        _logger.LogInformation("Fetching active landing page content");

        var content = await _contentDataStore.GetActiveContentAsync();
        if (content == null)
        {
            _logger.LogWarning("No active landing page content found");
            return new LandingPageResponse
            {
                PageTitle = "BridgeNow Finance",
                HeroText = "Your bridge to financial freedom",
                CtaText = "Apply Now"
            };
        }

        return new LandingPageResponse
        {
            PageTitle = content.PageTitle,
            HeroText = content.HeroText,
            CtaText = content.CtaText,
            BrandingAssetUrl = content.BrandingAssetUrl,
            ProductHighlights = content.ProductHighlights,
            EligibilityCriteria = content.EligibilityCriteria
        };
    }
}