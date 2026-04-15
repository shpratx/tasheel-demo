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

        var response = exception switch
        {
            ValidationException validationEx => new ErrorResponse
            {
                Timestamp = DateTime.UtcNow,
                TraceId = context.TraceIdentifier,
                ErrorCode = "VALIDATION_ERROR",
                Message = validationEx.Message,
                Details = validationEx.Errors.SelectMany(e => e.Value).ToList()
            },
            NotFoundException notFoundEx => new ErrorResponse
            {
                Timestamp = DateTime.UtcNow,
                TraceId = context.TraceIdentifier,
                ErrorCode = "NOT_FOUND",
                Message = notFoundEx.Message
            },
            EligibilityException eligibilityEx => new ErrorResponse
            {
                Timestamp = DateTime.UtcNow,
                TraceId = context.TraceIdentifier,
                ErrorCode = "ELIGIBILITY_FAILED",
                Message = eligibilityEx.Message,
                Details = eligibilityEx.FailedRules
            },
            UnauthorizedAccessException => new ErrorResponse
            {
                Timestamp = DateTime.UtcNow,
                TraceId = context.TraceIdentifier,
                ErrorCode = "UNAUTHORIZED",
                Message = "Unauthorized access"
            },
            WorkflowException workflowEx => new ErrorResponse
            {
                Timestamp = DateTime.UtcNow,
                TraceId = context.TraceIdentifier,
                ErrorCode = "WORKFLOW_ERROR",
                Message = workflowEx.Message
            },
            _ => new ErrorResponse
            {
                Timestamp = DateTime.UtcNow,
                TraceId = context.TraceIdentifier,
                ErrorCode = "INTERNAL_ERROR",
                Message = "An internal server error occurred"
            }
        };

        context.Response.StatusCode = exception switch
        {
            ValidationException => (int)HttpStatusCode.BadRequest,
            EligibilityException => (int)HttpStatusCode.BadRequest,
            NotFoundException => (int)HttpStatusCode.NotFound,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}

public static class GlobalExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}