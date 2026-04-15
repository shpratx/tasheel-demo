using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyProject.Controllers;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;
using Xunit;

namespace MyProject.Tests.Controllers;

public class EligibilityControllerTests
{
    private readonly Mock<IEligibilityService> _mockEligibilityService;
    private readonly Mock<ILogger<EligibilityController>> _mockLogger;
    private readonly EligibilityController _controller;

    public EligibilityControllerTests()
    {
        _mockEligibilityService = new Mock<IEligibilityService>();
        _mockLogger = new Mock<ILogger<EligibilityController>>();
        _controller = new EligibilityController(_mockEligibilityService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CheckEligibility_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var request = new EligibilityRequest
        {
            CustomerId = Guid.NewGuid(),
            LoanAmount = 100000,
            AnnualIncome = 75000,
            CreditScore = 750
        };

        var response = new EligibilityResponse
        {
            IsEligible = true,
            IsStpEligible = true,
            Status = "STP_ELIGIBLE",
            EvaluatedDate = DateTime.UtcNow
        };

        _mockEligibilityService.Setup(s => s.CheckEligibilityAsync(It.IsAny<EligibilityRequest>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.CheckEligibility(request);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult!.Value.Should().BeEquivalentTo(response);
    }
}