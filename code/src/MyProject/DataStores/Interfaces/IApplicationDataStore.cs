using MyProject.Models.Entities;

namespace MyProject.DataStores.Interfaces;

public interface IApplicationDataStore
{
    Task<Application?> GetByIdAsync(Guid applicationId);
    Task<IEnumerable<Application>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<Application>> GetStpEligibleApplicationsAsync();
    Task<IEnumerable<Application>> GetApplicationsForManualReviewAsync();
    Task<Application> CreateAsync(Application application);
    Task<Application> UpdateAsync(Application application);
    Task<bool> DeleteAsync(Guid applicationId);
}