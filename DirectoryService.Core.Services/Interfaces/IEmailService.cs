using DirectoryService.Core.Entities;

namespace DirectoryService.Core.Services.Interfaces;

public interface IEmailService
{
    public Task ClearSentEmails();
    public Task QueueNewEmail(QueuedEmail queuedEmail);
    public Task<List<QueuedEmail>> GetQueuedEmails(int limit = 1000);
    public Task SendEmails();
}