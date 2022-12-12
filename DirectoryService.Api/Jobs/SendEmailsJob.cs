using DirectoryService.Core.Services.Interfaces;
using FluentScheduler;

namespace DirectoryService.Api.Jobs;

/// <summary>
/// This job is responsible for calling upon the SendEmails method in the EmailService
/// </summary>
public class SendEmailsJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public SendEmailsJob (IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async void Execute()
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var emailService = serviceScope.ServiceProvider.GetRequiredService<IEmailService>();
        await emailService.SendEmails();
    }
}