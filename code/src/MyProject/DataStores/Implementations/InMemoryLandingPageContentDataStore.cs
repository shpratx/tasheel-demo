using MyProject.DataStores.Interfaces;
using MyProject.Models.Entities;
using System.Collections.Concurrent;

namespace MyProject.DataStores.Implementations;

public class InMemoryLandingPageContentDataStore : ILandingPageContentDataStore
{
    private readonly ConcurrentDictionary<Guid, LandingPageContent> _contents = new();

    public InMemoryLandingPageContentDataStore()
    {
        // Seed with sample data
        SeedData();
    }

    private void SeedData()
    {
        var sampleContent = new LandingPageContent
        {
            Id = Guid.NewGuid(),
            PageTitle = "BridgeNow Finance - Your Bridge to Financial Freedom",
            HeroText = "Get approved for a bridge loan in minutes with our streamlined digital application process",
            CtaText = "Apply Now",
            BrandingAssetUrl = "https://example.com/assets/bridgenow-logo.png",
            ProductHighlights = "Fast approval, competitive rates, flexible terms",
            EligibilityCriteria = "Credit score 700+, stable income, property value verification",
            IsActive = true,
            Version = 1,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        _contents.TryAdd(sampleContent.Id, sampleContent);
    }

    public Task<LandingPageContent?> GetActiveContentAsync()
    {
        var content = _contents.Values.FirstOrDefault(c => c.IsActive);
        return Task.FromResult(content);
    }

    public Task<LandingPageContent?> GetByVersionAsync(int version)
    {
        var content = _contents.Values.FirstOrDefault(c => c.Version == version);
        return Task.FromResult(content);
    }

    public Task<LandingPageContent> CreateAsync(LandingPageContent content)
    {
        content.Id = Guid.NewGuid();
        content.CreatedDate = DateTime.UtcNow;
        content.UpdatedDate = DateTime.UtcNow;
        
        if (!_contents.TryAdd(content.Id, content))
        {
            throw new InvalidOperationException($"Failed to create landing page content with ID {content.Id}");
        }
        
        return Task.FromResult(content);
    }

    public Task<LandingPageContent> UpdateAsync(LandingPageContent content)
    {
        if (!_contents.ContainsKey(content.Id))
        {
            throw new KeyNotFoundException($"Landing page content with ID {content.Id} not found");
        }
        
        content.UpdatedDate = DateTime.UtcNow;
        _contents[content.Id] = content;
        return Task.FromResult(content);
    }
}