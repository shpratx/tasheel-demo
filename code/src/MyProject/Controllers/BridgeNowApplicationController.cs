using Microsoft.AspNetCore.Mvc;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Controllers;

[ApiController]
[Route("api/v1/bridgenow")]
public class BridgeNowApplicationController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly ILogger<BridgeNowApplicationController> _logger;

    public BridgeNowApplicationController(
        IApplicationService applicationService,
        ILogger<BridgeNowApplicationController> logger)
    {
        _applicationService = applicationService;
        _logger = logger;
    }

    /// <summary>
    /// Submit Application
    /// </summary>
    [HttpPost("applications")]
    [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SubmitApplication([FromBody] ApplicationRequest request)
    {
        _logger.LogInformation("Submitting application for customer {CustomerId}", request.CustomerId);
        var response = await _applicationService.SubmitApplicationAsync(request);
        return CreatedAtAction(nameof(GetApplicationStatus), new { applicationId = response.Id }, response);
    }

    /// <summary>
    /// Get Application Status
    /// </summary>
    [HttpGet("applications/{applicationId}/status")]
    [ProducesResponseType(typeof(ApplicationStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationStatus([FromRoute] Guid applicationId)
    {
        _logger.LogInformation("Getting status for application {ApplicationId}", applicationId);
        var response = await _applicationService.GetApplicationStatusAsync(applicationId);
        return Ok(response);
    }

    /// <summary>
    /// Update Application
    /// </summary>
    [HttpPut("applications/{applicationId}")]
    [ProducesResponseType(typeof(ApplicationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateApplication(
        [FromRoute] Guid applicationId,
        [FromBody] ApplicationUpdateRequest request)
    {
        _logger.LogInformation("Updating application {ApplicationId}", applicationId);
        var response = await _applicationService.UpdateApplicationAsync(applicationId, request);
        return Ok(response);
    }

    /// <summary>
    /// Trigger Manual Review
    /// </summary>
    [HttpPost("applications/{applicationId}/manual-review")]
    [ProducesResponseType(typeof(ManualReviewResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TriggerManualReview(
        [FromRoute] Guid applicationId,
        [FromBody] ManualReviewRequest request)
    {
        _logger.LogInformation("Triggering manual review for application {ApplicationId}", applicationId);
        
        // Get application status first
        var status = await _applicationService.GetApplicationStatusAsync(applicationId);
        
        var response = new ManualReviewResponse
        {
            ApplicationId = applicationId,
            ReviewStatus = "Assigned",
            AssignedReviewer = "System Reviewer",
            AssignedDate = DateTime.UtcNow
        };
        
        return CreatedAtAction(nameof(GetApplicationStatus), new { applicationId }, response);
    }
}