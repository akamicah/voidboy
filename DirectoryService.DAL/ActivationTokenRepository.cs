using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class ActivationTokenRepository : BaseRepository<ActivationToken>, IActivationTokenRepository
{
    public ActivationTokenRepository(DbContext dbContext) : base(dbContext)
    {
        TableName = "activationTokens";
    }

    public async Task<ActivationToken> Create(ActivationToken entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var id = await con.QuerySingleAsync<Guid>(
            @"INSERT INTO activationTokens (accountId, expires)
                VALUES( @accountId, @expires )
                RETURNING id;",
            new
            {
                entity.AccountId,
                entity.Expires,
            });

        return await Retrieve(id);
    }

    public async Task<ActivationToken?> Update(ActivationToken entity)
    {
        throw new NotImplementedException();
    }
    
    public async Task ExpireTokens()
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"DELETE FROM activationTokens WHERE expires < CURRENT_TIMESTAMP");
    }
}