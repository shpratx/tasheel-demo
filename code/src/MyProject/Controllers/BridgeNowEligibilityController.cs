using Microsoft.AspNetCore.Mvc;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Controllers;

[ApiController]
[Route("api/v1/bridgenow")]
public class BridgeNowEligibilityController : ControllerBase
{
    private readonly IEligibilityService _eligibilityService;
    private readonly ILogger<BridgeNowEligibilityController> _logger;

    public BridgeNowEligibilityController(
        IEligibilityService eligibilityService,
        ILogger<BridgeNowEligibilityController> logger)
    {
        _eligibilityService = eligibilityService;
        _logger = logger;
    }

    /// <summary>
    /// Check Eligibility
    /// </summary>
    [HttpPost("eligibility/check")]
    [ProducesResponseType(typeof(EligibilityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckEligibility([FromBody] EligibilityRequest request)
    {
        _logger.LogInformation("Checking eligibility for customer {CustomerId}", request.CustomerId);
        var response = await _eligibilityService.CheckEligibilityAsync(request);
        return StatusCode(StatusCodes.Status201Created, response);
    }
}