using MyProject.DataStores.Interfaces;
using MyProject.Exceptions;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductDataStore _productDataStore;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IProductDataStore productDataStore,
        ILogger<ProductService> logger)
    {
        _productDataStore = productDataStore;
        _logger = logger;
    }

    public async Task<ProductDetailsResponse> GetProductDetailsAsync(Guid productId)
    {
        _logger.LogInformation("Retrieving product details for product {ProductId}", productId);

        var product = await _productDataStore.GetByIdAsync(productId);
        if (product == null)
        {
            throw new NotFoundException($"Product {productId} not found");
        }

        return new ProductDetailsResponse
        {
            Id = product.Id,
            ProductCode = product.ProductCode,
            ProductName = product.ProductName,
            Description = product.Description ?? string.Empty,
            MinLoanAmount = product.MinLoanAmount,
            MaxLoanAmount = product.MaxLoanAmount,
            InterestRate = product.InterestRate,
            ProcessingFee = product.ProcessingFee,
            IsActive = product.IsActive,
            CreatedDate = product.CreatedDate,
            UpdatedDate = product.UpdatedDate
        };
    }
}