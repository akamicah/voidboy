using System.Net;
using Serilog;

namespace DirectoryService.Api.Middleware;

/// <summary>
/// Handle HTTP error codes encountered in a nice way
/// consistent with API returns
/// </summary>
public class StatusCodeExceptionHandler
{
    private readonly RequestDelegate _request;

    public StatusCodeExceptionHandler(RequestDelegate pipeline)
    {
        _request = pipeline;
    }

    public Task Invoke(HttpContext context) =>
        InvokeAsync(context);

    private async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _request(context);
            switch (context.Response.StatusCode)
            {
                case 404:
                    Log.Warning("Request to unknown path: {path} using {method} from {ip}", context.Request.Path, context.Request.Method, context.Connection.RemoteIpAddress);
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Status = "failure",
                        Error = "UnknownMethod"
                    });
                    break;
                case 405:
                    Log.Warning("Request of illegal method: {path} using {method} from {ip}", context.Request.Path, context.Request.Method, context.Connection.RemoteIpAddress);
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Status = "failure",
                        Error = "IllegalMethod",
                    });
                    break;
            }
        }
        catch (StatusCodeException exception)
        {
            context.Response.StatusCode = (int) exception.StatusCode;
            context.Response.Headers.Clear();
        }
    }
}

public abstract class StatusCodeException : Exception
{
    protected StatusCodeException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; set; }
}

public static class StatusCodeExceptionHandlerExtensions
{
    public static IApplicationBuilder UseStatusCodeExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<StatusCodeExceptionHandler>();
    }
}