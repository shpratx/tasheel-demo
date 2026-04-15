using System.ComponentModel.DataAnnotations;

namespace MyProject.Models.Dtos;

public class EligibilityRequest
{
    [Required]
    public Guid CustomerId { get; set; }
    public decimal? LoanAmount { get; set; }
    public decimal? AnnualIncome { get; set; }
    public string? EmploymentStatus { get; set; }
    public int? CreditScore { get; set; }
    public decimal? PropertyValue { get; set; }
}