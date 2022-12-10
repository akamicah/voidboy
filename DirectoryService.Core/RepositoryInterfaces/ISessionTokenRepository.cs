using DirectoryService.Core.Entities;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface ISessionTokenRepository : IIdentifiedInterface<SessionToken>
{
    public Task<SessionToken?> FindByRefreshToken(Guid refreshToken);
    public Task ExpireTokens();
}