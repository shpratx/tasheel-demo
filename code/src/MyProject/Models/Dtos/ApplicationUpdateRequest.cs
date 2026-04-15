using System.ComponentModel.DataAnnotations;

namespace MyProject.Models.Dtos;

public class ApplicationUpdateRequest
{
    [MaxLength(50)]
    public string? FirstName { get; set; }

    [MaxLength(50)]
    public string? LastName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    public decimal? LoanAmount { get; set; }
    public decimal? AnnualIncome { get; set; }
    public string? EmploymentStatus { get; set; }
    public int? CreditScore { get; set; }
    public decimal? PropertyValue { get; set; }
}