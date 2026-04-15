using MyProject.DataStores.Interfaces;
using MyProject.Models.Entities;
using System.Collections.Concurrent;

namespace MyProject.DataStores.Implementations;

public class InMemoryProductDataStore : IProductDataStore
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();

    public InMemoryProductDataStore()
    {
        // Seed with sample data
        SeedData();
    }

    private void SeedData()
    {
        var sampleProduct = new Product
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            ProductCode = "BRIDGE-001",
            ProductName = "BridgeNow Standard Loan",
            Description = "Standard bridge loan product with competitive rates",
            MinLoanAmount = 10000m,
            MaxLoanAmount = 500000m,
            InterestRate = 5.5m,
            ProcessingFee = 500m,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        _products.TryAdd(sampleProduct.Id, sampleProduct);
    }

    public Task<Product?> GetByIdAsync(Guid productId)
    {
        _products.TryGetValue(productId, out var product);
        return Task.FromResult(product);
    }

    public Task<Product?> GetByProductCodeAsync(string productCode)
    {
        var product = _products.Values.FirstOrDefault(p => p.ProductCode.Equals(productCode, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(product);
    }

    public Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        var products = _products.Values.Where(p => p.IsActive).ToList();
        return Task.FromResult<IEnumerable<Product>>(products);
    }

    public Task<Product> CreateAsync(Product product)
    {
        product.Id = Guid.NewGuid();
        product.CreatedDate = DateTime.UtcNow;
        product.UpdatedDate = DateTime.UtcNow;
        
        if (!_products.TryAdd(product.Id, product))
        {
            throw new InvalidOperationException($"Failed to create product with ID {product.Id}");
        }
        
        return Task.FromResult(product);
    }

    public Task<Product> UpdateAsync(Product product)
    {
        if (!_products.ContainsKey(product.Id))
        {
            throw new KeyNotFoundException($"Product with ID {product.Id} not found");
        }
        
        product.UpdatedDate = DateTime.UtcNow;
        _products[product.Id] = product;
        return Task.FromResult(product);
    }
}