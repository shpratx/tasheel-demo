using MyProject.DataStores.Interfaces;
using MyProject.Models.Entities;
using System.Collections.Concurrent;

namespace MyProject.DataStores.Implementations;

public class InMemoryCommunicationTemplateDataStore : ICommunicationTemplateDataStore
{
    private readonly ConcurrentDictionary<Guid, CommunicationTemplate> _templates = new();

    public Task<CommunicationTemplate?> GetByTemplateCodeAsync(string templateCode)
    {
        var template = _templates.Values.FirstOrDefault(t => t.TemplateCode.Equals(templateCode, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(template);
    }

    public Task<IEnumerable<CommunicationTemplate>> GetByChannelAsync(CommunicationChannel channel)
    {
        var templates = _templates.Values.Where(t => t.Channel == channel);
        return Task.FromResult(templates);
    }

    public Task<IEnumerable<CommunicationTemplate>> GetApprovedTemplatesAsync()
    {
        var templates = _templates.Values.Where(t => t.ComplianceApproved && t.IsActive);
        return Task.FromResult(templates);
    }

    public Task<IEnumerable<CommunicationTemplate>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<CommunicationTemplate>>(_templates.Values);
    }

    public Task<CommunicationTemplate> CreateAsync(CommunicationTemplate template)
    {
        template.Id = Guid.NewGuid();
        template.CreatedDate = DateTime.UtcNow;
        template.UpdatedDate = DateTime.UtcNow;
        _templates.TryAdd(template.Id, template);
        return Task.FromResult(template);
    }

    public Task<CommunicationTemplate> UpdateAsync(CommunicationTemplate template)
    {
        template.UpdatedDate = DateTime.UtcNow;
        _templates[template.Id] = template;
        return Task.FromResult(template);
    }
}