namespace DirectoryService.Core.RepositoryInterfaces;

public interface IBaseInterface<T>
{
    public Task<T?> Create(T entity);
}

public interface IIdentifiedInterface<T> : IBaseInterface<T>
{
    public Task<T?> Retrieve(Guid id);
    public Task<T?> Update(T entity);
    public Task Delete(T entity);
    public Task Delete(IEnumerable<T> entities);
    public Task HardDelete(T entity);
    public Task HardDelete(IEnumerable<T> entities);
    public Task PurgeDeleted();
}
