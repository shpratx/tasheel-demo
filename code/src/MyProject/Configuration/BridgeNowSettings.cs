namespace MyProject.Configuration;

public class BridgeNowSettings
{
    public StpEligibilitySettings? StpEligibility { get; set; }
}

public class StpEligibilitySettings
{
    public int MinCreditScore { get; set; } = 700;
    public decimal MaxDebtToIncomeRatio { get; set; } = 0.4m;
    public decimal MinAnnualIncome { get; set; } = 50000m;
    public decimal MaxLoanToValueRatio { get; set; } = 0.8m;
}