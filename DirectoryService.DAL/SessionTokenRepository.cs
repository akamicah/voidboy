using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedRegistration]
public class SessionTokenRepository : ISessionTokenRepository
{
    private readonly DbContext _dbContext;

    public SessionTokenRepository(DbContext db)
    {
        _dbContext = db;
    }
    
    /// <summary>
    /// Create a new session token and store it in the db
    /// </summary>
    public async Task<SessionToken?> Create(SessionToken entity)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        var id = await con.QuerySingleAsync<Guid>(
            @"INSERT INTO sessionTokens (accountId, scope, expires)
                VALUES( @accountId, @scope, @expires )
                RETURNING id;",
            new
            {
                entity.AccountId,
                Scope = entity.Scope,
                entity.Expires,
            });

        return await Retrieve(id);
    }

    public async Task<SessionToken?> Retrieve(Guid id)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        var entity = await con.QueryFirstOrDefaultAsync<SessionToken>(
            @"SELECT * FROM sessionTokens WHERE id = :id",
            new
            {
                id
            });

        return entity;
    }
    
    public async Task<SessionToken?> Update(SessionToken entity)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(SessionToken entity)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        await con.ExecuteAsync(
            @"DELETE FROM sessionTokens WHERE id = :id",
            new
            {
                entity.Id
            });
    }

    public async Task Delete(IEnumerable<SessionToken> entities)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        await con.ExecuteAsync(
            @"DELETE FROM sessionTokens WHERE id = :id",
            entities.Select(e => e.Id)
                .Select(i => new { Id = i }).ToArray());
    }

    public async Task<SessionToken?> FindByRefreshToken(Guid refreshToken)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        var entity = await con.QueryFirstOrDefaultAsync<SessionToken>(
            @"SELECT * FROM sessionTokens WHERE refreshToken = :refreshToken",
            new
            {
                refreshToken
            });

        return entity;
    }

    public async Task ExpireTokens()
    {
        using var con = await _dbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"DELETE FROM sessionTokens WHERE expires < CURRENT_TIMESTAMP");
    }
    
    public async Task<PaginatedResponse<SessionToken>> ListAccountTokens(Guid accountId, PaginatedRequest page)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        var result = await con.QueryAsync<SessionToken>(
            @"SELECT * FROM sessionTokens WHERE accountId = @accountId ORDER BY createdAt LIMIT @pageSize OFFSET @offset",
            new
            {
                accountId,
                pageSize = page.PageSize,
                offset = (page.Page - 1) * page.PageSize
            });

        return new PaginatedResponse<SessionToken>()
        {
            Data = result,
            Page = page.Page,
            PageSize = page.PageSize
        };
    }
}