using System.Net;
using FluentValidation;
using DirectoryService.Core.Exceptions;

namespace DirectoryService.Api.Middleware;

/// <summary>
/// Handle exceptions in a nice way to not feed back stack traces to the user
/// </summary>
public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;
    
    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (BaseApiException ex)
        {
            await HandleApiExceptionAsync(httpContext, ex);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong: {ex}", ex);
            await HandleOtherExceptionAsync(httpContext);
        }
    }
    
    /// <summary>
    /// An *ApiException occured
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    private static async Task HandleApiExceptionAsync(HttpContext context, BaseApiException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception.ApiStatusCode();
        await context.Response.WriteAsJsonAsync(new
        {
            Status = "fail",
            Error = exception.ApiErrorCode(),
            Message = exception.ApiErrorMessage()
        });
    }
    
    /// <summary>
    /// Validation failed
    /// </summary>
    private static async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        await context.Response.WriteAsJsonAsync(new
        {
            Status = "fail",
            Error = "ValidationError",
            Message = exception.Errors.First().ErrorMessage
        });
    }
    
    /// <summary>
    /// Something else went wrong (a 500 error)
    /// </summary>
    /// <param name="context"></param>
    private static async Task HandleOtherExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            Status = "fail",
            Error = "Unknown",
            Message = "Something went wrong. Try again later."
        });
    }
}

public static class ApiExceptionHandlerExtensions
{
    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiExceptionMiddleware>();
    }
}