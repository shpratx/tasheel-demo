using MyProject.Models.Dtos;

namespace MyProject.Services.Interfaces;

public interface ICommunicationService
{
    Task<IEnumerable<CommunicationTemplateDto>> GetCommunicationTemplatesAsync();
}