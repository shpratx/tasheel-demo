using Microsoft.AspNetCore.Mvc;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Controllers;

[ApiController]
[Route("api/v1/bridgenow")]
public class BridgeNowCommunicationController : ControllerBase
{
    private readonly ICommunicationService _communicationService;
    private readonly ILogger<BridgeNowCommunicationController> _logger;

    public BridgeNowCommunicationController(
        ICommunicationService communicationService,
        ILogger<BridgeNowCommunicationController> logger)
    {
        _communicationService = communicationService;
        _logger = logger;
    }

    /// <summary>
    /// Get Communication Templates
    /// </summary>
    [HttpGet("communications/templates")]
    [ProducesResponseType(typeof(IEnumerable<CommunicationTemplateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCommunicationTemplates()
    {
        _logger.LogInformation("Getting communication templates");
        var response = await _communicationService.GetCommunicationTemplatesAsync();
        return Ok(response);
    }
}