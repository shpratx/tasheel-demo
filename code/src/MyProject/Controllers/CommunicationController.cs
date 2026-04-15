using Microsoft.AspNetCore.Mvc;
using MyProject.Services.Interfaces;

namespace MyProject.Controllers;

[ApiController]
[Route("v1/bridgenow/communications")]
public class CommunicationController : ControllerBase
{
    private readonly ICommunicationService _communicationService;
    private readonly ILogger<CommunicationController> _logger;

    public CommunicationController(
        ICommunicationService communicationService,
        ILogger<CommunicationController> logger)
    {
        _communicationService = communicationService;
        _logger = logger;
    }

    [HttpGet("templates")]
    public async Task<IActionResult> GetCommunicationTemplates()
    {
        _logger.LogInformation("Getting all communication templates");
        var response = await _communicationService.GetCommunicationTemplatesAsync();
        return Ok(response);
    }
}