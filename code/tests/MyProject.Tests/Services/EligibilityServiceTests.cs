using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MyProject.DataStores.Interfaces;
using MyProject.Models.Dtos;
using MyProject.Models.Entities;
using MyProject.Services.Implementations;
using Xunit;

namespace MyProject.Tests.Services;

public class EligibilityServiceTests
{
    private readonly Mock<ICustomerDataStore> _mockCustomerDataStore;
    private readonly Mock<ILogger<EligibilityService>> _mockLogger;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly EligibilityService _service;

    public EligibilityServiceTests()
    {
        _mockCustomerDataStore = new Mock<ICustomerDataStore>();
        _mockLogger = new Mock<ILogger<EligibilityService>>();
        _mockConfiguration = new Mock<IConfiguration>();

        var configSection = new Mock<IConfigurationSection>();
        configSection.Setup(x => x.GetSection("StpEligibility")).Returns(new Mock<IConfigurationSection>().Object);
        _mockConfiguration.Setup(x => x.GetSection("BridgeNowSettings")).Returns(configSection.Object);

        _service = new EligibilityService(
            _mockCustomerDataStore.Object,
            _mockLogger.Object,
            _mockConfiguration.Object);
    }

    [Fact]
    public async Task CheckEligibilityAsync_HighCreditScore_ReturnsStpEligible()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId,
            CreditScore = 750,
            DebtToIncomeRatio = 0.3m
        };

        var request = new EligibilityRequest
        {
            CustomerId = customerId,
            LoanAmount = 100000,
            AnnualIncome = 80000,
            EmploymentStatus = "Employed",
            CreditScore = 750
        };

        _mockCustomerDataStore.Setup(s => s.GetByIdAsync(customerId))
            .ReturnsAsync(customer);

        // Act
        var result = await _service.CheckEligibilityAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.IsEligible.Should().BeTrue();
        result.IsStpEligible.Should().BeTrue();
    }

    [Fact]
    public async Task CheckEligibilityAsync_LowCreditScore_ReturnsNotStpEligible()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId,
            CreditScore = 650,
            DebtToIncomeRatio = 0.3m
        };

        var request = new EligibilityRequest
        {
            CustomerId = customerId,
            LoanAmount = 100000,
            AnnualIncome = 80000,
            EmploymentStatus = "Employed",
            CreditScore = 650
        };

        _mockCustomerDataStore.Setup(s => s.GetByIdAsync(customerId))
            .ReturnsAsync(customer);

        // Act
        var result = await _service.CheckEligibilityAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.IsEligible.Should().BeTrue();
        result.IsStpEligible.Should().BeFalse();
    }
}