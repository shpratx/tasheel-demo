using MyProject.DataStores.Interfaces;
using MyProject.Models.Entities;
using System.Collections.Concurrent;

namespace MyProject.DataStores.Implementations;

public class InMemoryCommunicationTemplateDataStore : ICommunicationTemplateDataStore
{
    private readonly ConcurrentDictionary<Guid, CommunicationTemplateEntity> _templates = new();

    public InMemoryCommunicationTemplateDataStore()
    {
        // Seed with sample data
        SeedData();
    }

    private void SeedData()
    {
        var emailTemplate = new CommunicationTemplateEntity
        {
            Id = Guid.NewGuid(),
            TemplateCode = "APP_CONFIRMATION",
            TemplateName = "Application Confirmation Email",
            Channel = CommunicationChannel.Email,
            Subject = "Your BridgeNow Finance Application Received",
            Body = "Dear {FirstName}, Thank you for submitting your application. Your application ID is {ApplicationId}.",
            IsActive = true,
            ComplianceApproved = true,
            ApprovedDate = DateTime.UtcNow.AddDays(-30),
            CreatedDate = DateTime.UtcNow.AddDays(-30),
            UpdatedDate = DateTime.UtcNow.AddDays(-30)
        };
        _templates.TryAdd(emailTemplate.Id, emailTemplate);

        var smsTemplate = new CommunicationTemplateEntity
        {
            Id = Guid.NewGuid(),
            TemplateCode = "APP_APPROVED",
            TemplateName = "Application Approved SMS",
            Channel = CommunicationChannel.SMS,
            Subject = null,
            Body = "Congratulations! Your BridgeNow Finance application has been approved. Application ID: {ApplicationId}",
            IsActive = true,
            ComplianceApproved = true,
            ApprovedDate = DateTime.UtcNow.AddDays(-30),
            CreatedDate = DateTime.UtcNow.AddDays(-30),
            UpdatedDate = DateTime.UtcNow.AddDays(-30)
        };
        _templates.TryAdd(smsTemplate.Id, smsTemplate);
    }

    public Task<CommunicationTemplateEntity?> GetByTemplateCodeAsync(string templateCode)
    {
        var template = _templates.Values.FirstOrDefault(t => t.TemplateCode.Equals(templateCode, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(template);
    }

    public Task<IEnumerable<CommunicationTemplateEntity>> GetByChannelAsync(CommunicationChannel channel)
    {
        var templates = _templates.Values.Where(t => t.Channel == channel).ToList();
        return Task.FromResult<IEnumerable<CommunicationTemplateEntity>>(templates);
    }

    public Task<IEnumerable<CommunicationTemplateEntity>> GetApprovedTemplatesAsync()
    {
        var templates = _templates.Values.Where(t => t.IsActive && t.ComplianceApproved).ToList();
        return Task.FromResult<IEnumerable<CommunicationTemplateEntity>>(templates);
    }

    public Task<IEnumerable<CommunicationTemplateEntity>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<CommunicationTemplateEntity>>(_templates.Values.ToList());
    }

    public Task<CommunicationTemplateEntity> CreateAsync(CommunicationTemplateEntity template)
    {
        template.Id = Guid.NewGuid();
        template.CreatedDate = DateTime.UtcNow;
        template.UpdatedDate = DateTime.UtcNow;
        
        if (!_templates.TryAdd(template.Id, template))
        {
            throw new InvalidOperationException($"Failed to create communication template with ID {template.Id}");
        }
        
        return Task.FromResult(template);
    }

    public Task<CommunicationTemplateEntity> UpdateAsync(CommunicationTemplateEntity template)
    {
        if (!_templates.ContainsKey(template.Id))
        {
            throw new KeyNotFoundException($"Communication template with ID {template.Id} not found");
        }
        
        template.UpdatedDate = DateTime.UtcNow;
        _templates[template.Id] = template;
        return Task.FromResult(template);
    }
}