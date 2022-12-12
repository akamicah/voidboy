using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.Core.Services;

[ScopedRegistration]
public class TokenExpiryService
{
    private readonly IActivationTokenRepository _activationTokenRepository;
    private readonly ISessionTokenRepository _sessionTokenRepository;

    public TokenExpiryService(IActivationTokenRepository activationTokenRepository,
        ISessionTokenRepository sessionTokenRepository)
    {
        _activationTokenRepository = activationTokenRepository;
        _sessionTokenRepository = sessionTokenRepository;
    }

    /// <summary>
    /// Delete expired tokens
    /// </summary>
    public async Task ExpireTokens()
    {
        await _activationTokenRepository.ExpireTokens();
        await _sessionTokenRepository.ExpireTokens();
    }
}