using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyProject.Controllers;
using MyProject.Models.Dtos;
using MyProject.Services.Interfaces;
using Xunit;

namespace MyProject.Tests.Controllers;

public class ApplicationControllerTests
{
    private readonly Mock<IApplicationService> _mockApplicationService;
    private readonly Mock<ILogger<ApplicationController>> _mockLogger;
    private readonly ApplicationController _controller;

    public ApplicationControllerTests()
    {
        _mockApplicationService = new Mock<IApplicationService>();
        _mockLogger = new Mock<ILogger<ApplicationController>>();
        _controller = new ApplicationController(_mockApplicationService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task SubmitApplication_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var request = new ApplicationRequest
        {
            CustomerId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "+1234567890",
            LoanAmount = 100000,
            AnnualIncome = 75000,
            EmploymentStatus = "Employed"
        };

        var response = new ApplicationResponse
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            FirstName = request.FirstName,
            Status = "Submitted"
        };

        _mockApplicationService.Setup(s => s.SubmitApplicationAsync(It.IsAny<ApplicationRequest>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.SubmitApplication(request);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult!.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task GetApplicationStatus_ExistingApplication_ReturnsOkResult()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        var statusResponse = new ApplicationStatusResponse
        {
            ApplicationId = applicationId,
            Status = "Submitted",
            LastUpdated = DateTime.UtcNow
        };

        _mockApplicationService.Setup(s => s.GetApplicationStatusAsync(applicationId))
            .ReturnsAsync(statusResponse);

        // Act
        var result = await _controller.GetApplicationStatus(applicationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(statusResponse);
    }

    [Fact]
    public async Task UpdateApplication_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        var updateRequest = new ApplicationUpdateRequest
        {
            FirstName = "Jane",
            LoanAmount = 150000
        };

        var response = new ApplicationResponse
        {
            Id = applicationId,
            FirstName = "Jane",
            LoanAmount = 150000
        };

        _mockApplicationService.Setup(s => s.UpdateApplicationAsync(applicationId, It.IsAny<ApplicationUpdateRequest>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.UpdateApplication(applicationId, updateRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(response);
    }
}