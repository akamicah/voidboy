using Dapper;
using DirectoryService.DAL.Infrastructure;

namespace DirectoryService.DAL;

public class BaseRepository<T>
{
    protected string? TableName { get; set; }
    protected readonly DbContext DbContext;

    protected BaseRepository(DbContext db)
    {
        DbContext = db;
    }

    public async Task<T?> Retrieve(Guid id)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var sql = $@"SELECT * FROM {TableName} WHERE id = @id";
        var entity = await con.QueryFirstOrDefaultAsync<T>(sql,new { id });
        return entity;
    }
    
    public async Task Delete(Guid entityId)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var sql = $@"DELETE FROM {TableName} WHERE id = @id";
        await con.ExecuteAsync(
            sql, new { Id = entityId });
    }

    public async Task Delete(IEnumerable<Guid> entityIds)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var sql = $@"DELETE FROM {TableName} WHERE id = @id";
        await con.ExecuteAsync(sql, entityIds.Select(e => e));
    }
}