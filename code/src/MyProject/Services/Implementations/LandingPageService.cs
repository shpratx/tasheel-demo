using MyProject.DataStores.Interfaces;
using MyProject.Exceptions;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Services.Implementations;

public class LandingPageService : ILandingPageService
{
    private readonly ILandingPageContentDataStore _landingPageContentDataStore;
    private readonly ILogger<LandingPageService> _logger;

    public LandingPageService(
        ILandingPageContentDataStore landingPageContentDataStore,
        ILogger<LandingPageService> logger)
    {
        _landingPageContentDataStore = landingPageContentDataStore;
        _logger = logger;
    }

    public async Task<LandingPageResponse> GetLandingPageContentAsync()
    {
        _logger.LogInformation("Retrieving landing page content");

        var content = await _landingPageContentDataStore.GetActiveContentAsync();
        if (content == null)
        {
            throw new NotFoundException("Active landing page content not found");
        }

        return new LandingPageResponse
        {
            PageTitle = content.PageTitle,
            HeroText = content.HeroText,
            CtaText = content.CtaText,
            BrandingAssetUrl = content.BrandingAssetUrl ?? string.Empty,
            ProductHighlights = content.ProductHighlights ?? string.Empty,
            EligibilityCriteria = content.EligibilityCriteria ?? string.Empty
        };
    }
}