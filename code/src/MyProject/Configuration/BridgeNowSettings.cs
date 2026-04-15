namespace MyProject.Configuration;

public class BridgeNowSettings
{
    public StpEligibilitySettings StpEligibility { get; set; } = new();
    public WorkflowSettings WorkflowSettings { get; set; } = new();
}

public class StpEligibilitySettings
{
    public int MinCreditScore { get; set; } = 700;
    public decimal MaxDebtToIncomeRatio { get; set; } = 0.4m;
    public decimal MinAnnualIncome { get; set; } = 50000;
    public decimal MaxLoanToValueRatio { get; set; } = 0.8m;
}

public class WorkflowSettings
{
    public int StpProcessingTimeoutMinutes { get; set; } = 5;
    public int ManualReviewAssignmentDelaySeconds { get; set; } = 30;
}