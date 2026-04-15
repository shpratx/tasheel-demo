namespace MyProject.Models.Dtos;

public class ApplicationResponse
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public decimal LoanAmount { get; set; }
    public decimal AnnualIncome { get; set; }
    public string EmploymentStatus { get; set; } = string.Empty;
    public int? CreditScore { get; set; }
    public decimal? PropertyValue { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsStpEligible { get; set; }
    public string? EligibilityStatus { get; set; }
    public string? CurrentStage { get; set; }
    public DateTime SubmittedDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? ReviewAssignedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}