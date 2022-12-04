using DirectoryService.Api.Providers;

namespace DirectoryService.Api.Middleware.Authentication;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, SessionProvider sessionProvider)
    {
        // Used to populate the Session HttpContext item for authentication
        await sessionProvider.GetRequestSession();
        await _next(context);
    }
}