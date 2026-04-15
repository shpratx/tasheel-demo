using MyProject.Models.Entities;

namespace MyProject.DataStores.Interfaces;

public interface ICustomerDataStore
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetBySsnAsync(string ssn);
    Task<Customer> CreateAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(Guid id);
}