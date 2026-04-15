namespace MyProject.Models.Dtos;

public class EligibilityResponse
{
    public bool IsEligible { get; set; }
    public bool IsStpEligible { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<RuleResult> RuleResults { get; set; } = new();
    public DateTime EvaluatedDate { get; set; }
}

public class RuleResult
{
    public string RuleName { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public string Message { get; set; } = string.Empty;
}