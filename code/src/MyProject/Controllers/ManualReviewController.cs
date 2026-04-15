using Microsoft.AspNetCore.Mvc;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;

namespace MyProject.Controllers;

[ApiController]
[Route("v1/bridgenow/applications")]
public class ManualReviewController : ControllerBase
{
    private readonly IManualReviewService _manualReviewService;
    private readonly ILogger<ManualReviewController> _logger;

    public ManualReviewController(
        IManualReviewService manualReviewService,
        ILogger<ManualReviewController> logger)
    {
        _manualReviewService = manualReviewService;
        _logger = logger;
    }

    [HttpPost("{applicationId}/manual-review")]
    public async Task<IActionResult> TriggerManualReview(Guid applicationId, [FromBody] ManualReviewRequest request)
    {
        _logger.LogInformation("Triggering manual review for application {ApplicationId}", applicationId);
        var response = await _manualReviewService.TriggerManualReviewAsync(applicationId, request);
        return CreatedAtAction(nameof(TriggerManualReview), response);
    }
}