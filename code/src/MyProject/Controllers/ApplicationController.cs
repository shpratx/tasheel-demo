using Microsoft.AspNetCore.Mvc;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Controllers;

[ApiController]
[Route("v1/bridgenow/applications")]
public class ApplicationController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly ILogger<ApplicationController> _logger;

    public ApplicationController(
        IApplicationService applicationService,
        ILogger<ApplicationController> logger)
    {
        _applicationService = applicationService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitApplication([FromBody] ApplicationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Submitting application for customer {CustomerId}", request.CustomerId);
        var response = await _applicationService.SubmitApplicationAsync(request);
        return CreatedAtAction(nameof(GetApplicationStatus), new { applicationId = response.Id }, response);
    }

    [HttpGet("{applicationId}/status")]
    public async Task<IActionResult> GetApplicationStatus(Guid applicationId)
    {
        _logger.LogInformation("Getting application status for {ApplicationId}", applicationId);
        var response = await _applicationService.GetApplicationStatusAsync(applicationId);
        return Ok(response);
    }

    [HttpPut("{applicationId}")]
    public async Task<IActionResult> UpdateApplication(Guid applicationId, [FromBody] ApplicationUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Updating application {ApplicationId}", applicationId);
        var response = await _applicationService.UpdateApplicationAsync(applicationId, request);
        return Ok(response);
    }
}