using Microsoft.AspNetCore.Mvc;
using MyProject.Services.Interfaces;

namespace MyProject.Controllers;

[ApiController]
[Route("v1/bridgenow")]
public class LandingPageController : ControllerBase
{
    private readonly ILandingPageService _landingPageService;
    private readonly ILogger<LandingPageController> _logger;

    public LandingPageController(
        ILandingPageService landingPageService,
        ILogger<LandingPageController> logger)
    {
        _landingPageService = landingPageService;
        _logger = logger;
    }

    [HttpGet("landing-page")]
    public async Task<IActionResult> GetLandingPageContent()
    {
        _logger.LogInformation("Getting landing page content");
        var response = await _landingPageService.GetLandingPageContentAsync();
        return Ok(response);
    }
}