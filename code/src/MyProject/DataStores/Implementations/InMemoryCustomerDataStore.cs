using MyProject.DataStores.Interfaces;
using MyProject.Models.Entities;
using System.Collections.Concurrent;

namespace MyProject.DataStores.Implementations;

public class InMemoryCustomerDataStore : ICustomerDataStore
{
    private readonly ConcurrentDictionary<Guid, Customer> _customers = new();

    public Task<Customer?> GetByIdAsync(Guid id)
    {
        _customers.TryGetValue(id, out var customer);
        return Task.FromResult(customer);
    }

    public Task<Customer?> GetByEmailAsync(string email)
    {
        var customer = _customers.Values.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(customer);
    }

    public Task<Customer?> GetBySsnAsync(string ssn)
    {
        var customer = _customers.Values.FirstOrDefault(c => c.SSN == ssn);
        return Task.FromResult(customer);
    }

    public Task<Customer> CreateAsync(Customer customer)
    {
        customer.Id = Guid.NewGuid();
        customer.CreatedDate = DateTime.UtcNow;
        customer.UpdatedDate = DateTime.UtcNow;
        _customers.TryAdd(customer.Id, customer);
        return Task.FromResult(customer);
    }

    public Task<Customer> UpdateAsync(Customer customer)
    {
        customer.UpdatedDate = DateTime.UtcNow;
        _customers[customer.Id] = customer;
        return Task.FromResult(customer);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return Task.FromResult(_customers.TryRemove(id, out _));
    }
}