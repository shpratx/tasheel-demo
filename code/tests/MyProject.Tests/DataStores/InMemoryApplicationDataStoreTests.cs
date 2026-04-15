using FluentAssertions;
using MyProject.DataStores.Implementations;
using MyProject.Models.Entities;
using Xunit;

namespace MyProject.Tests.DataStores;

public class InMemoryApplicationDataStoreTests
{
    private readonly InMemoryApplicationDataStore _dataStore;

    public InMemoryApplicationDataStoreTests()
    {
        _dataStore = new InMemoryApplicationDataStore();
    }

    [Fact]
    public async Task CreateAsync_ValidApplication_ReturnsCreatedApplication()
    {
        // Arrange
        var application = new Application
        {
            CustomerId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "+1234567890",
            LoanAmount = 100000,
            AnnualIncome = 75000,
            EmploymentStatus = "Employed",
            Status = ApplicationStatus.Submitted,
            SubmittedDate = DateTime.UtcNow
        };

        // Act
        var result = await _dataStore.CreateAsync(application);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task GetByIdAsync_ExistingApplication_ReturnsApplication()
    {
        // Arrange
        var application = new Application
        {
            CustomerId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "+1234567890",
            LoanAmount = 100000,
            AnnualIncome = 75000,
            EmploymentStatus = "Employed",
            Status = ApplicationStatus.Submitted,
            SubmittedDate = DateTime.UtcNow
        };

        var created = await _dataStore.CreateAsync(application);

        // Act
        var result = await _dataStore.GetByIdAsync(created.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(created.Id);
        result.FirstName.Should().Be("John");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingApplication_ReturnsNull()
    {
        // Act
        var result = await _dataStore.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ExistingApplication_ReturnsUpdatedApplication()
    {
        // Arrange
        var application = new Application
        {
            CustomerId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "+1234567890",
            LoanAmount = 100000,
            AnnualIncome = 75000,
            EmploymentStatus = "Employed",
            Status = ApplicationStatus.Submitted,
            SubmittedDate = DateTime.UtcNow
        };

        var created = await _dataStore.CreateAsync(application);
        created.Status = ApplicationStatus.Approved;

        // Act
        var result = await _dataStore.UpdateAsync(created);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(ApplicationStatus.Approved);
        result.UpdatedDate.Should().BeAfter(result.CreatedDate);
    }

    [Fact]
    public async Task GetStpEligibleApplicationsAsync_ReturnsOnlyStpEligible()
    {
        // Arrange
        var stpApp = new Application
        {
            CustomerId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "+1234567890",
            LoanAmount = 100000,
            AnnualIncome = 75000,
            EmploymentStatus = "Employed",
            Status = ApplicationStatus.Submitted,
            IsStpEligible = true,
            SubmittedDate = DateTime.UtcNow
        };

        var nonStpApp = new Application
        {
            CustomerId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane@example.com",
            PhoneNumber = "+1234567891",
            LoanAmount = 50000,
            AnnualIncome = 40000,
            EmploymentStatus = "Employed",
            Status = ApplicationStatus.Submitted,
            IsStpEligible = false,
            SubmittedDate = DateTime.UtcNow
        };

        await _dataStore.CreateAsync(stpApp);
        await _dataStore.CreateAsync(nonStpApp);

        // Act
        var result = await _dataStore.GetStpEligibleApplicationsAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().FirstName.Should().Be("John");
    }
}