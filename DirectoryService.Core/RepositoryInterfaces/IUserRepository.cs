using DirectoryService.Core.Entities;
using DirectoryService.Shared;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IUserRepository : IBaseRepository<User>
{
    public Task<User?> FindByUsername(string username);
    public Task<User?> FindByEmail(string emailAddress);
    public Task<PaginatedResult<User>> ListRelativeUsers(Guid relativeUser, PaginatedRequest page, bool includeSelf);
}