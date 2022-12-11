using FluentScheduler;

namespace DirectoryService.Api.Jobs;

/// <summary>
/// Define scheduled jobs to run at specific intervals
/// </summary>
public class SchedulerRegistry : Registry
{
    public SchedulerRegistry(IServiceScopeFactory serviceScopeFactory)
    {
        Schedule(() => new SendEmailsJob(serviceScopeFactory)).ToRunNow().AndEvery(15).Seconds();
        Schedule(() => new TokenExpiryJob(serviceScopeFactory)).ToRunNow().AndEvery(1).Minutes();
    }
}