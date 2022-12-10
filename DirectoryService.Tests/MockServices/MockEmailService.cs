using DirectoryService.Core.Entities;
using DirectoryService.Core.Services.Interfaces;

namespace DirectoryService.Tests.MockServices;

public class MockEmailService : IEmailService
{
    private readonly List<QueuedEmail> _queuedEmails;

    public MockEmailService()
    {
        _queuedEmails = new List<QueuedEmail>();
    }

    public async Task QueueNewEmail(QueuedEmail queuedEmail)
    {
        _queuedEmails.Add(queuedEmail);
    }

    public async Task SendEmails()
    {
        // Do nothing..... yet
    }

    public async Task<List<QueuedEmail>> GetQueuedEmails(int limit = 1000)
    {
        return _queuedEmails;
    }
}