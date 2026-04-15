using MyProject.Models.Entities;

namespace MyProject.DataStores.Interfaces;

public interface ICommunicationTemplateDataStore
{
    Task<CommunicationTemplateEntity?> GetByTemplateCodeAsync(string templateCode);
    Task<IEnumerable<CommunicationTemplateEntity>> GetByChannelAsync(CommunicationChannel channel);
    Task<IEnumerable<CommunicationTemplateEntity>> GetApprovedTemplatesAsync();
    Task<IEnumerable<CommunicationTemplateEntity>> GetAllAsync();
    Task<CommunicationTemplateEntity> CreateAsync(CommunicationTemplateEntity template);
    Task<CommunicationTemplateEntity> UpdateAsync(CommunicationTemplateEntity template);
}