using DirectoryService.Core.Entities;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.Core.Services;

[ScopedDependency]
public class SessionTokenService
{
    private readonly IActivationTokenRepository _activationTokenRepository;
    private readonly ISessionTokenRepository _sessionTokenRepository;
    private readonly ISessionProvider _sessionProvider;

    public SessionTokenService(IActivationTokenRepository activationTokenRepository,
        ISessionTokenRepository sessionTokenRepository,
        ISessionProvider sessionProvider)
    {
        _activationTokenRepository = activationTokenRepository;
        _sessionTokenRepository = sessionTokenRepository;
        _sessionProvider = sessionProvider;
    }

    /// <summary>
    /// Delete expired tokens
    /// </summary>
    public async Task ExpireTokens()
    {
        await _activationTokenRepository.ExpireTokens();
        await _sessionTokenRepository.ExpireTokens();
    }

    /// <summary>
    /// List all session tokens for user
    /// </summary>
    public async Task<PaginatedResult<SessionToken>> ListUserTokens(Guid userId, PaginatedRequest page)
    {
        var session = await _sessionProvider.GetRequesterSession();
        
        if(session is null) throw new UnauthorisedApiException();
        if (session.UserId != userId && !session.AsAdmin) throw new UnauthorisedApiException();
        
        page.Where.Add("accountId", userId);
        page.OrderBy = "createdAt";
        var result = await _sessionTokenRepository.List(page);
        return result;
    }

    /// <summary>
    /// Request to revoke a token for a user
    /// </summary>
    public async Task RevokeUserToken(Guid userId, Guid token)
    {
        var session = await _sessionProvider.GetRequesterSession();
        
        if(session is null) throw new UnauthorisedApiException();
        if (session.UserId != userId && !session.AsAdmin) throw new UnauthorisedApiException();
        
        var tokenEntity = await _sessionTokenRepository.Retrieve(token);

        if (tokenEntity == null)
            throw new InvalidTokenApiException();
        
        if(tokenEntity.UserId != userId)
            throw new InvalidTokenApiException();

        await _sessionTokenRepository.Delete(token);
    }
}