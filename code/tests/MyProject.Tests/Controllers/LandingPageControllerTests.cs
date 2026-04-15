using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyProject.Controllers;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;
using Xunit;

namespace MyProject.Tests.Controllers;

public class LandingPageControllerTests
{
    private readonly Mock<ILandingPageService> _mockLandingPageService;
    private readonly Mock<ILogger<LandingPageController>> _mockLogger;
    private readonly LandingPageController _controller;

    public LandingPageControllerTests()
    {
        _mockLandingPageService = new Mock<ILandingPageService>();
        _mockLogger = new Mock<ILogger<LandingPageController>>();
        _controller = new LandingPageController(_mockLandingPageService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetLandingPageContent_ReturnsOkResult()
    {
        // Arrange
        var response = new LandingPageResponse
        {
            PageTitle = "BridgeNow Finance",
            HeroText = "Your bridge to financial freedom",
            CtaText = "Apply Now"
        };

        _mockLandingPageService.Setup(s => s.GetLandingPageContentAsync())
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetLandingPageContent();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(response);
    }
}