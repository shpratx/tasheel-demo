using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MyProject.DataStores.Interfaces;
using MyProject.Exceptions;
using MyProject.Models.Dtos;
using MyProject.Models.Entities;
using MyProject.Services.Implementations;
using MyProject.Services.Interfaces;
using Xunit;

namespace MyProject.Tests.Services;

public class ApplicationServiceTests
{
    private readonly Mock<IApplicationDataStore> _mockApplicationDataStore;
    private readonly Mock<ICustomerDataStore> _mockCustomerDataStore;
    private readonly Mock<IEligibilityService> _mockEligibilityService;
    private readonly Mock<IWorkflowService> _mockWorkflowService;
    private readonly Mock<ILogger<ApplicationService>> _mockLogger;
    private readonly ApplicationService _service;

    public ApplicationServiceTests()
    {
        _mockApplicationDataStore = new Mock<IApplicationDataStore>();
        _mockCustomerDataStore = new Mock<ICustomerDataStore>();
        _mockEligibilityService = new Mock<IEligibilityService>();
        _mockWorkflowService = new Mock<IWorkflowService>();
        _mockLogger = new Mock<ILogger<ApplicationService>>();
        _service = new ApplicationService(
            _mockApplicationDataStore.Object,
            _mockCustomerDataStore.Object,
            _mockEligibilityService.Object,
            _mockWorkflowService.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task SubmitApplicationAsync_ValidRequest_ReturnsApplicationResponse()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            CreditScore = 750,
            DebtToIncomeRatio = 0.3m
        };

        var request = new ApplicationRequest
        {
            CustomerId = customerId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "+1234567890",
            LoanAmount = 100000,
            AnnualIncome = 75000,
            EmploymentStatus = "Employed"
        };

        var eligibilityResponse = new EligibilityResponse
        {
            IsEligible = true,
            IsStpEligible = true,
            Status = "STP_ELIGIBLE"
        };

        var savedApplication = new Application
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            FirstName = request.FirstName,
            Status = ApplicationStatus.Submitted,
            IsStpEligible = true,
            SubmittedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _mockCustomerDataStore.Setup(s => s.GetByIdAsync(customerId))
            .ReturnsAsync(customer);
        _mockEligibilityService.Setup(s => s.CheckEligibilityAsync(It.IsAny<EligibilityRequest>()))
            .ReturnsAsync(eligibilityResponse);
        _mockApplicationDataStore.Setup(s => s.CreateAsync(It.IsAny<Application>()))
            .ReturnsAsync(savedApplication);

        // Act
        var result = await _service.SubmitApplicationAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(savedApplication.Id);
        result.Status.Should().Be("Submitted");
    }

    [Fact]
    public async Task SubmitApplicationAsync_CustomerNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var request = new ApplicationRequest
        {
            CustomerId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "+1234567890",
            LoanAmount = 100000,
            AnnualIncome = 75000,
            EmploymentStatus = "Employed"
        };

        _mockCustomerDataStore.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Customer?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.SubmitApplicationAsync(request));
    }

    [Fact]
    public async Task GetApplicationStatusAsync_ExistingApplication_ReturnsStatus()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        var application = new Application
        {
            Id = applicationId,
            Status = ApplicationStatus.Approved,
            UpdatedDate = DateTime.UtcNow,
            IsStpEligible = true,
            CurrentStage = "APPROVED"
        };

        _mockApplicationDataStore.Setup(s => s.GetByIdAsync(applicationId))
            .ReturnsAsync(application);

        // Act
        var result = await _service.GetApplicationStatusAsync(applicationId);

        // Assert
        result.Should().NotBeNull();
        result.ApplicationId.Should().Be(applicationId);
        result.Status.Should().Be("Approved");
    }
}