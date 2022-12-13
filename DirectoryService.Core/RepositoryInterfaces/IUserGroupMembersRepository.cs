using DirectoryService.Core.Entities;
using DirectoryService.Shared;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IUserGroupMembersRepository : IBaseRepository<User>
{
    public Task<PaginatedResult<User>> List(Guid groupId, PaginatedRequest page);
    public Task Add(Guid groupId, Guid userId);
    public Task Add(Guid groupId, IEnumerable<Guid> userIds);
    public Task Delete(Guid groupId, Guid userId);
    public Task Delete(Guid groupId, IEnumerable<Guid> userId);
}