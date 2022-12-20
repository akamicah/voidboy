using DirectoryService.Core.Entities;
using DirectoryService.Shared;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IPlaceManagerRepository : IBaseRepository<User>
{
    public Task<PaginatedResult<User>> List(Guid placeId, PaginatedRequest page);
    public Task Add(Guid placeId, Guid userId);
    public Task Add(Guid placeId, IEnumerable<Guid> userIds);
    public Task Delete(Guid placeId, Guid userId);
    public Task Delete(Guid placeId, IEnumerable<Guid> userId);
}