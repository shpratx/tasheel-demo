using MyProject.Models.Entities;

namespace MyProject.DataStores.Interfaces;

public interface IProductDataStore
{
    Task<Product?> GetByIdAsync(Guid productId);
    Task<Product?> GetByProductCodeAsync(string productCode);
    Task<IEnumerable<Product>> GetActiveProductsAsync();
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
}