using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedRegistration]
public class EmailQueueRepository : IEmailQueueRepository
{
    private readonly DbContext _dbContext;

    public EmailQueueRepository(DbContext db)
    {
        _dbContext = db;
    }
    
    public async Task<QueuedEmail?> Create(QueuedEmail entity)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        var id = await con.QuerySingleAsync<Guid>(
            @"INSERT INTO emailQueue (accountId, model, type, sendOn)
                VALUES( @accountId, @model, @type, @sendOn )
                RETURNING id;",
            new
            {
                entity.AccountId,
                entity.Model,
                entity.Type,
                entity.SendOn
            });

        return await Retrieve(id);
    }
    
    public async Task<QueuedEmail?> Retrieve(Guid id)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        var entity = await con.QueryFirstOrDefaultAsync<QueuedEmail>(
            @"SELECT * FROM emailQueue WHERE id = :id AND deleted = FALSE",
            new
            {
                id
            });

        return entity;
    }

    public async Task<IEnumerable<QueuedEmail>> GetNextQueuedEmails(int limit = 1000)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        var emails = await con.QueryAsync<QueuedEmail>(
            @"SELECT * FROM emailQueue WHERE deleted = FALSE AND sent = FALSE
                 AND sendOn < CURRENT_TIMESTAMP ORDER BY sendOn LIMIT @limit",
            new
            {
                limit
            });
        return emails;
    }

    
    public async Task<QueuedEmail?> Update(QueuedEmail entity)
    {
        throw new NotImplementedException();
    }

    public async Task SoftDelete(QueuedEmail entity)
    {
        throw new NotImplementedException();
    }

    public async Task SoftDelete(IEnumerable<QueuedEmail> entities)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(QueuedEmail entity)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(IEnumerable<QueuedEmail> entities)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Delete all entities where deleted = true
    /// </summary>
    public async Task PurgeDeleted()
    {
        using var con = await _dbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"DELETE FROM emailQueue WHERE deleted = TRUE");
    }


}