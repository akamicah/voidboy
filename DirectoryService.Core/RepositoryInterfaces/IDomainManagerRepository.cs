using DirectoryService.Core.Entities;
using DirectoryService.Shared;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IDomainManagerRepository : IBaseRepository<User>
{
    public Task<PaginatedResult<User>> List(Guid domainId, PaginatedRequest page);
    public Task Add(Guid domainId, Guid userId);
    public Task Add(Guid domainId, IEnumerable<Guid> userIds);
    public Task Delete(Guid domainId, Guid userId);
    public Task Delete(Guid domainId, IEnumerable<Guid> userId);
}