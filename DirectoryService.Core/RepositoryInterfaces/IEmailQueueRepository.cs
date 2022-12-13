using DirectoryService.Core.Entities;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IEmailQueueEntityRepository : IBaseRepository<QueuedEmail>
{
    public Task<IEnumerable<QueuedEmail>> GetNextQueuedEmails(int limit = 1000);
    public Task ClearSentEmails(DateTime cutoffDate);
}