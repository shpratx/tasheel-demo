using MyProject.DataStores.Interfaces;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Services.Implementations;

public class CommunicationService : ICommunicationService
{
    private readonly ICommunicationTemplateDataStore _templateDataStore;
    private readonly ILogger<CommunicationService> _logger;

    public CommunicationService(
        ICommunicationTemplateDataStore templateDataStore,
        ILogger<CommunicationService> logger)
    {
        _templateDataStore = templateDataStore;
        _logger = logger;
    }

    public async Task<List<CommunicationTemplate>> GetCommunicationTemplatesAsync()
    {
        _logger.LogInformation("Fetching all communication templates");

        var templates = await _templateDataStore.GetAllAsync();

        return templates.Select(t => new CommunicationTemplate
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
        }).ToList();
    }
}