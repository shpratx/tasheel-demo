using MyProject.Models.Entities;

namespace MyProject.DataStores.Interfaces;

public interface ICustomerDataStore
{
    Task<Customer?> GetByIdAsync(Guid customerId);
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer> CreateAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
}