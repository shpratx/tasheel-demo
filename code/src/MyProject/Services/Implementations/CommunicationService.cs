using MyProject.DataStores.Interfaces;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Services.Implementations;

public class CommunicationService : ICommunicationService
{
    private readonly ICommunicationTemplateDataStore _communicationTemplateDataStore;
    private readonly ILogger<CommunicationService> _logger;

    public CommunicationService(
        ICommunicationTemplateDataStore communicationTemplateDataStore,
        ILogger<CommunicationService> logger)
    {
        _communicationTemplateDataStore = communicationTemplateDataStore;
        _logger = logger;
    }

    public async Task<IEnumerable<CommunicationTemplateDto>> GetCommunicationTemplatesAsync()
    {
        _logger.LogInformation("Retrieving communication templates");

        var templates = await _communicationTemplateDataStore.GetAllAsync();

        return templates.Select(t => new CommunicationTemplateDto
        {
            Id = t.Id,
            TemplateCode = t.TemplateCode,
            TemplateName = t.TemplateName,
            Channel = t.Channel.ToString(),
            Subject = t.Subject,
            Body = t.Body,
            IsActive = t.IsActive,
            ComplianceApproved = t.ComplianceApproved,
            ApprovedDate = t.ApprovedDate,
            CreatedDate = t.CreatedDate,
            UpdatedDate = t.UpdatedDate
        });
    }
}