using MyProject.Models.Dtos;

namespace MyProject.Services.Interfaces;

public interface IProductService
{
    Task<ProductDetailsResponse> GetProductDetailsAsync(Guid productId);
}