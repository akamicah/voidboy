using DirectoryService.Core.Entities;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IUserRepository : IBaseRepository<User>
{
    public Task<User?> FindByUsername(string username);
    public Task<User?> FindByEmail(string emailAddress);
}