namespace MyProject.Exceptions;

public class EligibilityException : Exception
{
    public List<string> FailedRules { get; }

    public EligibilityException(string message, List<string> failedRules)
        : base(message)
    {
        FailedRules = failedRules;
    }
}