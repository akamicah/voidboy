using DirectoryService.Core.Entities;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IActivationTokenRepository : IBaseRepository<ActivationToken>
{
    public Task ExpireTokens();
}