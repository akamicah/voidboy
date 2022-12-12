using DirectoryService.Core.Entities;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IActivationTokenRepository : IGuidIdentifiedEntityRepository<ActivationToken>
{
    public Task ExpireTokens();
}