using MyProject.Exceptions;
using MyProject.Models.Dtos;
using System.Net;

namespace MyProject.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, errorCode, message, details) = exception switch
        {
            ValidationException validationEx => (
                StatusCodes.Status400BadRequest,
                "VALIDATION_ERROR",
                validationEx.Message,
                validationEx.Errors.SelectMany(e => e.Value).ToList()
            ),
            NotFoundException notFoundEx => (
                StatusCodes.Status404NotFound,
                "NOT_FOUND",
                notFoundEx.Message,
                new List<string>()
            ),
            EligibilityException eligibilityEx => (
                StatusCodes.Status400BadRequest,
                "ELIGIBILITY_FAILED",
                eligibilityEx.Message,
                eligibilityEx.FailedRules
            ),
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "UNAUTHORIZED",
                "Unauthorized access",
                new List<string>()
            ),
            WorkflowException workflowEx => (
                StatusCodes.Status500InternalServerError,
                "WORKFLOW_ERROR",
                workflowEx.Message,
                new List<string>()
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "INTERNAL_ERROR",
                "An internal server error occurred",
                new List<string>()
            )
        };

        var response = new ErrorResponse
        {
            Timestamp = DateTime.UtcNow,
            TraceId = context.TraceIdentifier,
            ErrorCode = errorCode,
            Message = message,
            Details = details
        };

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(response);
    }
}