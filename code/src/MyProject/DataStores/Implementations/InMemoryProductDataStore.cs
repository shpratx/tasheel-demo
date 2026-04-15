using MyProject.DataStores.Interfaces;
using MyProject.Models.Entities;
using System.Collections.Concurrent;

namespace MyProject.DataStores.Implementations;

public class InMemoryProductDataStore : IProductDataStore
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();

    public Task<Product?> GetByIdAsync(Guid id)
    {
        _products.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }

    public Task<Product?> GetByProductCodeAsync(string productCode)
    {
        var product = _products.Values.FirstOrDefault(p => p.ProductCode.Equals(productCode, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(product);
    }

    public Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        var products = _products.Values.Where(p => p.IsActive);
        return Task.FromResult(products);
    }

    public Task<Product> CreateAsync(Product product)
    {
        product.Id = Guid.NewGuid();
        product.CreatedDate = DateTime.UtcNow;
        product.UpdatedDate = DateTime.UtcNow;
        _products.TryAdd(product.Id, product);
        return Task.FromResult(product);
    }

    public Task<Product> UpdateAsync(Product product)
    {
        product.UpdatedDate = DateTime.UtcNow;
        _products[product.Id] = product;
        return Task.FromResult(product);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return Task.FromResult(_products.TryRemove(id, out _));
    }
}