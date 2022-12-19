using DirectoryService.Core.Entities;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface ISessionTokenRepository : IBaseRepository<SessionToken>
{
    public Task<SessionToken?> FindByRefreshToken(Guid refreshToken);
    public Task ExpireTokens();
}