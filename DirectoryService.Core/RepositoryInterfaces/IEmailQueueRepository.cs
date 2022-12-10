using DirectoryService.Core.Entities;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IEmailQueueRepository : IIdentifiedInterface<QueuedEmail>
{
    public Task<IEnumerable<QueuedEmail>> GetNextQueuedEmails(int limit = 1000);
}