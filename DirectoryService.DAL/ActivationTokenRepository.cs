using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedRegistration]
public class ActivationTokenRepository : IActivationTokenRepository
{
    private readonly DbContext _dbContext;

    public ActivationTokenRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ActivationToken?> Create(ActivationToken entity)
    {
        using var con = await _dbContext.CreateConnectionAsync();
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
    
    public async Task<ActivationToken?> Retrieve(Guid id)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        var entity = await con.QueryFirstOrDefaultAsync<ActivationToken>(
            @"SELECT * FROM activationTokens WHERE id = :id",
            new
            {
                id
            });

        return entity;
    }
    
    public async Task<ActivationToken?> Update(ActivationToken entity)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(ActivationToken entity)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        await con.ExecuteAsync(
            @"DELETE FROM activationTokens WHERE id = :id",
            new
            {
                entity.Id
            });
    }

    public async Task Delete(IEnumerable<ActivationToken> entities)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        await con.ExecuteAsync(
            @"DELETE FROM activationTokens WHERE id = :id",
            entities.Select(e => e.Id)
                .Select(i => new { Id = i }).ToArray());
    }

    public async Task ExpireTokens()
    {
        using var con = await _dbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"DELETE FROM activationTokens WHERE expires < CURRENT_TIMESTAMP");
    }
}