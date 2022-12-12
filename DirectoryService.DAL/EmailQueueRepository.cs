using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedRegistration]
public class EmailQueueEntityRepository : BaseRepository<QueuedEmail>, IEmailQueueEntityRepository
{
    public EmailQueueEntityRepository(DbContext db) : base(db)
    {
        TableName = "emailQueue";
    }
    
    public async Task<QueuedEmail?> Create(QueuedEmail entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
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


    public async Task<IEnumerable<QueuedEmail>> GetNextQueuedEmails(int limit = 1000)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var emails = await con.QueryAsync<QueuedEmail>(
            @"SELECT * FROM emailQueue WHERE sent = FALSE
                 AND sendOn < CURRENT_TIMESTAMP ORDER BY sendOn LIMIT @limit",
            new
            {
                limit
            });
        return emails;
    }
    
    public async Task<QueuedEmail?> Update(QueuedEmail entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(
            @"UPDATE emailQueue SET sent = @sent, sentOn = @sentOn, sendOn = @sendOn, attempt = @attempt WHERE id = @id",
            new
            {
                entity.Id,
                entity.Sent,
                entity.SendOn,
                entity.SentOn,
                entity.Attempt
            });

        return await Retrieve(entity.Id);
    }

    public async Task ClearSentEmails(DateTime cutoffDate)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(
            @"DELETE FROM emailQueue WHERE sent=TRUE AND sentOn < @cutoffDate", new
            {
                cutoffDate
            });
    }
}