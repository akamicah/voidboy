using DirectoryService.Core.Entities;
using DirectoryService.Shared;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface ISessionTokenRepository : IBaseRepository<SessionToken>
{
    public Task<PaginatedResult<SessionToken>> ListAccountTokens(Guid accountId, PaginatedRequest page);
    public Task<SessionToken?> FindByRefreshToken(Guid refreshToken);
    public Task ExpireTokens();
}