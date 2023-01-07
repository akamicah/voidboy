using Dapper;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;

namespace DirectoryService.DAL;

public class BaseRepository<T>
{
    protected string? TableName { get; init; }
    protected readonly DbContext DbContext;

    protected BaseRepository(DbContext db)
    {
        DbContext = db;
    }

    protected async Task<PaginatedResult<T>> QueryDynamic(string sqlTemplate, string tableName, PaginatedRequest page,
        DynamicParameters? dynamicParameters = null)
    {
        using var con = await DbContext.CreateConnectionAsync();

        sqlTemplate += " WHERE 1=1";

        dynamicParameters ??= new DynamicParameters();

        var sqlWhere = "";
        var paramIx = 1;
        foreach (var (col, value) in page.Where.ToDictionary())
        {
            if (value.Count == 1)
            {
                sqlWhere += $@" AND {tableName}.{col} = @_p{paramIx}";
                dynamicParameters.Add("_p" + paramIx, value.First());
                paramIx += 1;
            }
            else
            {
                sqlWhere += $@" AND {tableName}.{col} IN ( ";
                foreach (var p in value)
                {
                    sqlWhere += $@"@_p{paramIx}, ";
                    dynamicParameters.Add("_p" + paramIx, p);
                    paramIx += 1;
                }

                sqlWhere = sqlWhere[^2..] + ")";
            }
        }

        if (page.Search != null && page.SearchOn != null)
        {
            var like = "%" + page.Search + "%";
            sqlWhere += $@" AND {tableName}.{page.SearchOn} LIKE @_p{paramIx}";
            dynamicParameters.Add("_p" + paramIx, like);
        }

        if (page.OrderBy != null)
        {
            sqlWhere += $@" ORDER BY {page.OrderBy}";
            if (!page.OrderAscending)
                sqlWhere += $@" DESC";
        }

        dynamicParameters.Add("_pOffset", (page.Page - 1) * page.PageSize);
        dynamicParameters.Add("_pLimit", page.PageSize);

        var resultTemplate = sqlTemplate + sqlWhere + " OFFSET @_pOffset LIMIT @_pLimit ";
        var countTemplate = "SELECT COUNT(*) FROM (" + sqlTemplate + sqlWhere + ") resultTotal;";
        var result = await con.QueryAsync<T>(resultTemplate, dynamicParameters);
        var total = await con.QuerySingleAsync<int>(countTemplate, dynamicParameters);

        return new PaginatedResult<T>()
        {
            Page = page.Page,
            PageSize = page.PageSize,
            TotalPages = total == 0 ? 0 : (int)Math.Round((double)(total / page.PageSize)) + 1,
            Total = total,
            Data = result
        };
    }

    public virtual async Task<PaginatedResult<T>> List(PaginatedRequest request)
    {
        return await QueryDynamic($@"SELECT * FROM {TableName} t", "t", request);
    }

    public virtual async Task<T?> Retrieve(Guid id)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var sql = $@"SELECT * FROM {TableName} WHERE id = @id";
        var entity = await con.QueryFirstOrDefaultAsync<T>(sql, new { id });
        return entity;
    }

    public virtual async Task Delete(Guid entityId)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var sql = $@"DELETE FROM {TableName} WHERE id = @id";
        await con.ExecuteAsync(
            sql, new { Id = entityId });
    }

    public virtual async Task Delete(IEnumerable<Guid> entityIds)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var sql = $@"DELETE FROM {TableName} WHERE id = @id";
        await con.ExecuteAsync(sql, entityIds);
    }
}