using System.ComponentModel.DataAnnotations;

namespace MyProject.Models.Entities;

public class Product
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string ProductCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ProductName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public decimal MinLoanAmount { get; set; }

    [Required]
    public decimal MaxLoanAmount { get; set; }

    [Required]
    public decimal InterestRate { get; set; }

    [Required]
    public decimal ProcessingFee { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime UpdatedDate { get; set; }
}