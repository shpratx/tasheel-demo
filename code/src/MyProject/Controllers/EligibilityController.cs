using Microsoft.AspNetCore.Mvc;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Controllers;

[ApiController]
[Route("v1/bridgenow/eligibility")]
public class EligibilityController : ControllerBase
{
    private readonly IEligibilityService _eligibilityService;
    private readonly ILogger<EligibilityController> _logger;

    public EligibilityController(
        IEligibilityService eligibilityService,
        ILogger<EligibilityController> logger)
    {
        _eligibilityService = eligibilityService;
        _logger = logger;
    }

    [HttpPost("check")]
    public async Task<IActionResult> CheckEligibility([FromBody] EligibilityRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Checking eligibility for customer {CustomerId}", request.CustomerId);
        var response = await _eligibilityService.CheckEligibilityAsync(request);
        return CreatedAtAction(nameof(CheckEligibility), response);
    }
}