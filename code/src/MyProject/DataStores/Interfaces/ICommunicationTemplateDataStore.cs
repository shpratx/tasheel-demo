using MyProject.Models.Entities;

namespace MyProject.DataStores.Interfaces;

public interface ICommunicationTemplateDataStore
{
    Task<CommunicationTemplate?> GetByTemplateCodeAsync(string templateCode);
    Task<IEnumerable<CommunicationTemplate>> GetByChannelAsync(CommunicationChannel channel);
    Task<IEnumerable<CommunicationTemplate>> GetApprovedTemplatesAsync();
    Task<IEnumerable<CommunicationTemplate>> GetAllAsync();
    Task<CommunicationTemplate> CreateAsync(CommunicationTemplate template);
    Task<CommunicationTemplate> UpdateAsync(CommunicationTemplate template);
}