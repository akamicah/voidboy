using FluentScheduler;

namespace DirectoryService.Api.Jobs;

/// <summary>
/// Define scheduled jobs to run at specific intervals
/// </summary>
public class JobRegistry : Registry
{
    public JobRegistry(IServiceScopeFactory serviceScopeFactory)
    {
        Schedule(() => new SendEmailsJob(serviceScopeFactory)).ToRunNow().AndEvery(5).Seconds();
        Schedule(() => new TokenExpiryJob(serviceScopeFactory)).ToRunNow().AndEvery(1).Minutes();
        Schedule(() => new ClearSentEmailsJob(serviceScopeFactory)).ToRunNow().AndEvery(1).Days();
    }
}