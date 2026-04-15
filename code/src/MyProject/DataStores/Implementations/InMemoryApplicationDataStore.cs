using MyProject.DataStores.Interfaces;
using MyProject.Models.Entities;
using System.Collections.Concurrent;

namespace MyProject.DataStores.Implementations;

public class InMemoryApplicationDataStore : IApplicationDataStore
{
    private readonly ConcurrentDictionary<Guid, Application> _applications = new();

    public Task<Application?> GetByIdAsync(Guid id)
    {
        _applications.TryGetValue(id, out var application);
        return Task.FromResult(application);
    }

    public Task<IEnumerable<Application>> GetByCustomerIdAsync(Guid customerId)
    {
        var applications = _applications.Values.Where(a => a.CustomerId == customerId);
        return Task.FromResult(applications);
    }

    public Task<IEnumerable<Application>> GetStpEligibleApplicationsAsync()
    {
        var applications = _applications.Values
            .Where(a => a.IsStpEligible && a.Status == ApplicationStatus.Submitted)
            .OrderBy(a => a.SubmittedDate);
        return Task.FromResult(applications);
    }

    public Task<IEnumerable<Application>> GetApplicationsForManualReviewAsync()
    {
        var applications = _applications.Values
            .Where(a => !a.IsStpEligible && a.Status == ApplicationStatus.UnderReview)
            .OrderBy(a => a.ReviewAssignedDate);
        return Task.FromResult(applications);
    }

    public Task<Application> CreateAsync(Application application)
    {
        application.Id = Guid.NewGuid();
        application.CreatedDate = DateTime.UtcNow;
        application.UpdatedDate = DateTime.UtcNow;
        _applications.TryAdd(application.Id, application);
        return Task.FromResult(application);
    }

    public Task<Application> UpdateAsync(Application application)
    {
        application.UpdatedDate = DateTime.UtcNow;
        _applications[application.Id] = application;
        return Task.FromResult(application);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return Task.FromResult(_applications.TryRemove(id, out _));
    }
}