using Microsoft.AspNetCore.Mvc;
using MyProject.Services.Interfaces;

namespace MyProject.Controllers;

[ApiController]
[Route("v1/bridgenow/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        IProductService productService,
        ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductDetails(Guid productId)
    {
        _logger.LogInformation("Getting product details for {ProductId}", productId);
        var response = await _productService.GetProductDetailsAsync(productId);
        return Ok(response);
    }
}