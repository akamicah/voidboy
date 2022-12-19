using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class SessionTokenRepository : BaseRepository<SessionToken>, ISessionTokenRepository
{
    public SessionTokenRepository(DbContext dbContext) : base(dbContext)
    {
        TableName = "sessionTokens";
    }
    
    /// <summary>
    /// Create a new session token and store it in the db
    /// </summary>
    public async Task<SessionToken> Create(SessionToken entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var id = await con.QuerySingleAsync<Guid>(
            @"INSERT INTO sessionTokens (userId, scope, expires)
                VALUES( @userId, @scope, @expires )
                RETURNING id;",
            new
            {
                entity.UserId,
                Scope = entity.Scope,
                entity.Expires,
            });

        return await Retrieve(id);
    }

    public async Task<SessionToken?> Update(SessionToken entity)
    {
        throw new NotImplementedException();
    }
    
    public async Task<SessionToken?> FindByRefreshToken(Guid refreshToken)
    {
        using var con = await DbContext.CreateConnectionAsync();
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
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"DELETE FROM sessionTokens WHERE expires < CURRENT_TIMESTAMP");
    }
}