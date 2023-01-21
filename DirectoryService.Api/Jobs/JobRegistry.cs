using FluentScheduler;

namespace DirectoryService.Api.Jobs;

/// <summary>
/// Define scheduled jobs to run at specific intervals
/// </summary>
public class JobRegistry : Registry
{
    public JobRegistry(IServiceScopeFactory serviceScopeFactory)
    {
        Schedule(() => new SendEmailsJob(serviceScopeFactory)).ToRunEvery(5).Seconds();
        Schedule(() => new TokenExpiryJob(serviceScopeFactory)).ToRunEvery(1).Minutes();
        Schedule(() => new ClearSentEmailsJob(serviceScopeFactory)).ToRunEvery(1).Hours();
        Schedule(() => new RecordStatisticsJob(serviceScopeFactory)).ToRunEvery(1).Hours();
    }
}