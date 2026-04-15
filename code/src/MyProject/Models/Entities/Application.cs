using System.ComponentModel.DataAnnotations;

namespace MyProject.Models.Entities;

public class Application
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public decimal LoanAmount { get; set; }

    [Required]
    public decimal AnnualIncome { get; set; }

    [Required]
    public ApplicationEmploymentStatus EmploymentStatus { get; set; }

    public int? CreditScore { get; set; }

    public decimal? PropertyValue { get; set; }

    [Required]
    public ApplicationStatus Status { get; set; }

    public bool IsStpEligible { get; set; }

    [MaxLength(50)]
    public string? EligibilityStatus { get; set; }

    [MaxLength(50)]
    public string? CurrentStage { get; set; }

    [Required]
    public DateTime SubmittedDate { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public DateTime? ReviewAssignedDate { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime UpdatedDate { get; set; }
}

public enum ApplicationStatus
{
    Submitted,
    Processing,
    UnderReview,
    Approved,
    Rejected,
    Disbursed
}

public enum ApplicationEmploymentStatus
{
    Employed,
    SelfEmployed,
    Unemployed,
    Retired
}