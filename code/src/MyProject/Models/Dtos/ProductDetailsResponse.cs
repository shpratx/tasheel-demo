namespace MyProject.Models.Dtos;

public class ProductDetailsResponse
{
    public Guid Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal MinLoanAmount { get; set; }
    public decimal MaxLoanAmount { get; set; }
    public decimal InterestRate { get; set; }
    public decimal ProcessingFee { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}