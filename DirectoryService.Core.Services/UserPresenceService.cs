using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Shared.Attributes;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Core.Services;

[ScopedDependency]
public class UserPresenceService
{
    private readonly ILogger<UserPresenceService> _logger;
    private readonly IUserPresenceRepository _userPresenceRepository;
    private readonly ISessionProvider _sessionProvider;

    public UserPresenceService(ILogger<UserPresenceService> logger,
        IUserPresenceRepository userPresenceRepository,
        ISessionProvider sessionProvider)
    {
        _logger = logger;
        _userPresenceRepository = userPresenceRepository;
        _sessionProvider = sessionProvider;
    }

    public async Task<UserPresence?> GetUserPresence(Guid userId)
    {
        return await _userPresenceRepository.Retrieve(userId);
    }

    public async Task<UserPresence?> UpdateUserPresence(UserPresence userPresence)
    {
        userPresence.LastHeartbeat = DateTime.Now;
        return await _userPresenceRepository.Create(userPresence);
    }
    
}