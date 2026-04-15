using MyProject.DataStores.Interfaces;
using MyProject.Models.Entities;
using System.Collections.Concurrent;

namespace MyProject.DataStores.Implementations;

public class InMemoryLandingPageContentDataStore : ILandingPageContentDataStore
{
    private readonly ConcurrentDictionary<Guid, LandingPageContent> _contents = new();

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
        _contents.TryAdd(content.Id, content);
        return Task.FromResult(content);
    }

    public Task<LandingPageContent> UpdateAsync(LandingPageContent content)
    {
        content.UpdatedDate = DateTime.UtcNow;
        _contents[content.Id] = content;
        return Task.FromResult(content);
    }
}