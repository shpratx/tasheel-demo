using MyProject.DataStores.Interfaces;
using MyProject.Models.Entities;
using System.Collections.Concurrent;

namespace MyProject.DataStores.Implementations;

public class InMemoryCustomerDataStore : ICustomerDataStore
{
    private readonly ConcurrentDictionary<Guid, Customer> _customers = new();

    public InMemoryCustomerDataStore()
    {
        // Seed with sample data
        SeedData();
    }

    private void SeedData()
    {
        var sampleCustomer = new Customer
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "+1234567890",
            DateOfBirth = new DateTime(1985, 5, 15),
            SSN = "123-45-6789",
            Address = "123 Main St",
            City = "New York",
            State = "NY",
            ZipCode = "10001",
            CreditScore = 750,
            DebtToIncomeRatio = 0.35m,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        _customers.TryAdd(sampleCustomer.Id, sampleCustomer);
    }

    public Task<Customer?> GetByIdAsync(Guid customerId)
    {
        _customers.TryGetValue(customerId, out var customer);
        return Task.FromResult(customer);
    }

    public Task<Customer?> GetByEmailAsync(string email)
    {
        var customer = _customers.Values.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(customer);
    }

    public Task<Customer> CreateAsync(Customer customer)
    {
        customer.Id = Guid.NewGuid();
        customer.CreatedDate = DateTime.UtcNow;
        customer.UpdatedDate = DateTime.UtcNow;
        
        if (!_customers.TryAdd(customer.Id, customer))
        {
            throw new InvalidOperationException($"Failed to create customer with ID {customer.Id}");
        }
        
        return Task.FromResult(customer);
    }

    public Task<Customer> UpdateAsync(Customer customer)
    {
        if (!_customers.ContainsKey(customer.Id))
        {
            throw new KeyNotFoundException($"Customer with ID {customer.Id} not found");
        }
        
        customer.UpdatedDate = DateTime.UtcNow;
        _customers[customer.Id] = customer;
        return Task.FromResult(customer);
    }
}