using MyProject.DataStores.Interfaces;
using MyProject.Models.Entities;
using System.Collections.Concurrent;

namespace MyProject.DataStores.Implementations;

public class InMemoryApplicationDataStore : IApplicationDataStore
{
    private readonly ConcurrentDictionary<Guid, Application> _applications = new();

    public Task<Application?> GetByIdAsync(Guid applicationId)
    {
        _applications.TryGetValue(applicationId, out var application);
        return Task.FromResult(application);
    }

    public Task<IEnumerable<Application>> GetByCustomerIdAsync(Guid customerId)
    {
        var applications = _applications.Values
            .Where(a => a.CustomerId == customerId)
            .ToList();
        return Task.FromResult<IEnumerable<Application>>(applications);
    }

    public Task<IEnumerable<Application>> GetStpEligibleApplicationsAsync()
    {
        var applications = _applications.Values
            .Where(a => a.IsStpEligible && a.Status == ApplicationStatus.Submitted)
            .OrderBy(a => a.SubmittedDate)
            .ToList();
        return Task.FromResult<IEnumerable<Application>>(applications);
    }

    public Task<IEnumerable<Application>> GetApplicationsForManualReviewAsync()
    {
        var applications = _applications.Values
            .Where(a => !a.IsStpEligible && a.Status == ApplicationStatus.UnderReview)
            .OrderBy(a => a.ReviewAssignedDate)
            .ToList();
        return Task.FromResult<IEnumerable<Application>>(applications);
    }

    public Task<Application> CreateAsync(Application application)
    {
        application.Id = Guid.NewGuid();
        application.CreatedDate = DateTime.UtcNow;
        application.UpdatedDate = DateTime.UtcNow;
        
        if (!_applications.TryAdd(application.Id, application))
        {
            throw new InvalidOperationException($"Failed to create application with ID {application.Id}");
        }
        
        return Task.FromResult(application);
    }

    public Task<Application> UpdateAsync(Application application)
    {
        if (!_applications.ContainsKey(application.Id))
        {
            throw new KeyNotFoundException($"Application with ID {application.Id} not found");
        }
        
        application.UpdatedDate = DateTime.UtcNow;
        _applications[application.Id] = application;
        return Task.FromResult(application);
    }

    public Task<bool> DeleteAsync(Guid applicationId)
    {
        return Task.FromResult(_applications.TryRemove(applicationId, out _));
    }
}