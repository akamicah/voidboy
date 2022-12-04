namespace DirectoryService.Core.RepositoryInterfaces;

public interface IBaseInterface<T>
{
    public Task<T?> Create(T entity);
}

public interface IIdentifiedInterface<T> : IBaseInterface<T>
{
    public Task<T?> Retrieve(Guid id);
    public Task<T?> Update(T entity);
    public Task SoftDelete(T entity);
    public Task SoftDelete(IEnumerable<T> entities);
    public Task Delete(T entity);
    public Task Delete(IEnumerable<T> entities);
    public Task PurgeDeleted();
}
