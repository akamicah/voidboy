using DirectoryService.Core.Services;
using FluentScheduler;

namespace DirectoryService.Api.Jobs;

public class RecordStatisticsJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RecordStatisticsJob (IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async void Execute()
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var statsService = serviceScope.ServiceProvider.GetRequiredService<StatsService>();
        await statsService.Record();
    }
}