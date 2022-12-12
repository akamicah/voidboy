using DirectoryService.Core.Services.Interfaces;
using FluentScheduler;

namespace DirectoryService.Api.Jobs;

/// <summary>
/// Clear sent emails that were sent > 30 days ago.
/// </summary>
public class ClearSentEmailsJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public ClearSentEmailsJob (IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async void Execute()
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var emailService = serviceScope.ServiceProvider.GetRequiredService<IEmailService>();
        await emailService.ClearSentEmails();
    }
}