using DirectoryService.Core.Entities;
using DirectoryService.Shared;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface ISessionTokenRepository : IGuidIdentifiedEntityRepository<SessionToken>
{
    public Task<PaginatedResponse<SessionToken>> ListAccountTokens(Guid accountId, PaginatedRequest page);
    public Task<SessionToken?> FindByRefreshToken(Guid refreshToken);
    public Task ExpireTokens();
}