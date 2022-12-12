using Dapper;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;

namespace DirectoryService.DAL;

public class BaseRepository<T>
{
    protected string? TableName { get; set; }
    protected readonly DbContext DbContext;

    protected BaseRepository(DbContext db)
    {
        DbContext = db;
    }

    protected async Task<PaginatedResponse<T>> QueryDynamic(string sqlTemplate, string tableName, PaginatedRequest page)
    {
        using var con = await DbContext.CreateConnectionAsync();

        sqlTemplate += " WHERE 1=1";
        
        var dynamicParameters = new DynamicParameters();
        var sqlWhere = "";
        var paramIx = 1;
        foreach (var (col, value) in page.Where.ToDictionary())
        {
            if (value.Count() == 1)
            {
                sqlWhere += $@" AND {tableName}.{col} = @p{paramIx}";
                dynamicParameters.Add("p" + paramIx, value.First());
                paramIx = +1;
            }
            else
            {
                sqlWhere += $@" AND {tableName}.{col} IN ( ";
                foreach (var p in value)
                {
                    sqlWhere += $@"@p{paramIx}, ";
                    dynamicParameters.Add("p" + paramIx, p);
                    paramIx = +1;
                }

                sqlWhere = sqlWhere[^2..] + ")";
            }
        }

        if (page.Search != null && page.SearchOn != null)
        {
            var like = "%" + page.Search + "%";
            sqlWhere += $@" AND {tableName}.{page.SearchOn} LIKE @p{paramIx}";
            dynamicParameters.Add("p" + paramIx, like);
        }

        if (page.OrderBy != null)
        {
            sqlWhere += $@" ORDER BY {page.OrderBy}";
            if (!page.OrderAscending)
                sqlWhere += $@" DESC";
        }
        
        sqlWhere += " OFFSET @pOffset LIMIT @pLimit ";
        dynamicParameters.Add("pOffset", (page.Page - 1) * page.PageSize );
        dynamicParameters.Add("pLimit", page.PageSize );

            sqlTemplate += sqlWhere;
        
        var result = await con.QueryAsync<T>(sqlTemplate, dynamicParameters);

        return new PaginatedResponse<T>()
        {
            Data = result
        };
    }

    public async Task<PaginatedResponse<T>> List(PaginatedRequest request)
    {
        return await QueryDynamic($@"SELECT * FROM {TableName} t", "t", request);
    }

    public async Task<T?> Retrieve(Guid id)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var sql = $@"SELECT * FROM {TableName} WHERE id = @id";
        var entity = await con.QueryFirstOrDefaultAsync<T>(sql, new { id });
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