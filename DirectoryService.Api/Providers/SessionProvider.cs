using System.Net.Http.Headers;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Shared;

// ReSharper disable ClassNeverInstantiated.Global

namespace DirectoryService.Api.Providers;

public class SessionProvider : ISessionProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISessionTokenRepository _sessionTokenRepository;
    private readonly IUserRepository _userRepository;

    public SessionProvider(IHttpContextAccessor httpContextAccessor,
        ISessionTokenRepository sessionTokenRepository,
        IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _sessionTokenRepository = sessionTokenRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Get session information for user making current request
    /// </summary>
    public async Task<Session?> GetRequesterSession()
    {
        if (_httpContextAccessor.HttpContext?.Items["Session"] != null)
        {
            return (Session)_httpContextAccessor.HttpContext.Items["Session"]!;
        }

        Guid token;
        var asAdmin = false;

        if (_httpContextAccessor.HttpContext is null)
            return null;
        try
        {
            var authHeader =
                AuthenticationHeaderValue.Parse(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"]);
            var tokenString = authHeader.Parameter;

            if (tokenString is null) return null;

            if (!Guid.TryParse(tokenString, out token)) return null;

            var asAdminParam =
                (_httpContextAccessor.HttpContext!.Request.Query).FirstOrDefault(x => x.Key.ToLower() == "asadmin");
            if (asAdminParam.Key != "" && bool.TryParse(asAdminParam.Value.ToString(), out var paramValue))
            {
                asAdmin = paramValue;
            }
        }
        catch
        {
            return null;
        }

        var sessionToken = await _sessionTokenRepository.Retrieve(token);
        if (sessionToken is null) return null;

        var user = await _userRepository.Retrieve(sessionToken.AccountId);
        if (user is null) return null;

        var session = new Session()
        {
            Token = sessionToken.Id,
            AccountId = sessionToken.AccountId,
            Scope = sessionToken.Scope,
            Role = user.Role,
            AsAdmin = user.Role == UserRole.Admin && asAdmin
        };

        _httpContextAccessor.HttpContext!.Items["Session"] = session;
        return session;
    }
}