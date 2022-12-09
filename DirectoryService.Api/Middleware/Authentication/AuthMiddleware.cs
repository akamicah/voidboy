using DirectoryService.Api.Providers;
using DirectoryService.Core.Services.Interfaces;

namespace DirectoryService.Api.Middleware.Authentication;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ISessionProvider sessionProvider)
    {
        // Used to populate the Session HttpContext item for authentication
        await sessionProvider.GetRequesterSession();
        await _next(context);
    }
}