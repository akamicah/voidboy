namespace DirectoryService.Core.RepositoryInterfaces;

public interface IBaseRepository<T>
{
    public Task<T?> Create(T entity);
}

public interface IGuidIdentifiedEntityRepository<T> : IBaseRepository<T>
{
    public Task<T?> Retrieve(Guid id);
    public Task<T?> Update(T entity);
    public Task Delete(T entity);
    public Task Delete(IEnumerable<T> entities);
}
