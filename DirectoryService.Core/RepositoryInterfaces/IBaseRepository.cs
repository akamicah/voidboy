using DirectoryService.Shared;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IBaseRepository<T>
{
    public Task<T?> Create(T entity);
    public Task<T?> Retrieve(Guid id);
    public Task<PaginatedResponse<T>> List(PaginatedRequest request);
    public Task<T?> Update(T entity);
    public Task Delete(Guid id);
    public Task Delete(IEnumerable<Guid> ids);
    
}
