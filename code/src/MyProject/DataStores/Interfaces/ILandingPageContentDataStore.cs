using MyProject.Models.Entities;

namespace MyProject.DataStores.Interfaces;

public interface ILandingPageContentDataStore
{
    Task<LandingPageContent?> GetActiveContentAsync();
    Task<LandingPageContent?> GetByVersionAsync(int version);
    Task<LandingPageContent> CreateAsync(LandingPageContent content);
    Task<LandingPageContent> UpdateAsync(LandingPageContent content);
}