using System.ComponentModel.DataAnnotations;

namespace MyProject.Models.Dtos;

public class ApplicationRequest
{
    [Required(ErrorMessage = "Customer ID is required and must be a valid GUID")]
    public Guid CustomerId { get; set; }

    [Required(ErrorMessage = "First name is required and must contain only letters")]
    [MaxLength(50)]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must contain only letters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required and must contain only letters")]
    [MaxLength(50)]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must contain only letters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Valid email address is required")]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Valid phone number is required")]
    [Phone]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Loan amount must be between $10,000 and $500,000")]
    [Range(10000, 500000, ErrorMessage = "Loan amount must be between $10,000 and $500,000")]
    public decimal LoanAmount { get; set; }

    [Required(ErrorMessage = "Annual income must be a positive value")]
    [Range(1, double.MaxValue, ErrorMessage = "Annual income must be a positive value")]
    public decimal AnnualIncome { get; set; }

    [Required(ErrorMessage = "Valid employment status is required")]
    public string EmploymentStatus { get; set; } = string.Empty;

    [Range(300, 850, ErrorMessage = "Credit score must be between 300 and 850")]
    public int? CreditScore { get; set; }

    public decimal? PropertyValue { get; set; }
}