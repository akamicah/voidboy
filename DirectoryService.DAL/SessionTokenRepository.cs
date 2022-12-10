using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
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
            @"SELECT * FROM sessionTokens WHERE id = :id AND deleted = FALSE",
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
        throw new NotImplementedException();
    }

    public async Task Delete(IEnumerable<SessionToken> entities)
    {
        throw new NotImplementedException();
    }

    public async Task HardDelete(SessionToken entity)
    {
        throw new NotImplementedException();
    }

    public async Task HardDelete(IEnumerable<SessionToken> entities)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Delete all entities where deleted = true
    /// </summary>
    public async Task PurgeDeleted()
    {
        using var con = await _dbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"DELETE FROM sessionTokens WHERE deleted = TRUE");
    }

    public async Task<SessionToken?> FindByRefreshToken(Guid refreshToken)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        var entity = await con.QueryFirstOrDefaultAsync<SessionToken>(
            @"SELECT * FROM sessionTokens WHERE refreshToken = :refreshToken AND deleted = FALSE",
            new
            {
                refreshToken
            });

        return entity;
    }

    public async Task ExpireTokens()
    {
        //TODO
    }
}