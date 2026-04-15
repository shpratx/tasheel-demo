using Microsoft.AspNetCore.Mvc;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Controllers;

[ApiController]
[Route("api/v1/bridgenow")]
public class BridgeNowProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<BridgeNowProductController> _logger;

    public BridgeNowProductController(
        IProductService productService,
        ILogger<BridgeNowProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Get Product Details
    /// </summary>
    [HttpGet("products/{productId}")]
    [ProducesResponseType(typeof(ProductDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductDetails([FromRoute] Guid productId)
    {
        _logger.LogInformation("Getting product details for product {ProductId}", productId);
        var response = await _productService.GetProductDetailsAsync(productId);
        return Ok(response);
    }
}