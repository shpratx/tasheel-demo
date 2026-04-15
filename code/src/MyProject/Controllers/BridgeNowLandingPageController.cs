using Microsoft.AspNetCore.Mvc;
using MyProject.Services.Interfaces;

namespace MyProject.Controllers;

[ApiController]
[Route("api/v1/bridgenow")]
public class BridgeNowLandingPageController : ControllerBase
{
    private readonly ILandingPageService _landingPageService;
    private readonly ILogger<BridgeNowLandingPageController> _logger;

    public BridgeNowLandingPageController(
        ILandingPageService landingPageService,
        ILogger<BridgeNowLandingPageController> logger)
    {
        _landingPageService = landingPageService;
        _logger = logger;
    }

    /// <summary>
    /// Get Landing Page Content
    /// </summary>
    [HttpGet("landing-page")]
    [ProducesResponseType(typeof(Models.Dtos.LandingPageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Models.Dtos.ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLandingPageContent()
    {
        _logger.LogInformation("Getting landing page content");
        var response = await _landingPageService.GetLandingPageContentAsync();
        return Ok(response);
    }
}