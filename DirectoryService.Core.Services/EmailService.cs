using DirectoryService.Core.Dto;
using DirectoryService.Core.Entities;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DirectoryService.Core.Services;

[ScopedRegistration]
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly IFluentEmail _fluentEmail;
    private readonly ServiceConfiguration _config;
    
    public EmailService(ILogger<EmailService> logger,
        IEmailQueueRepository emailQueueRepository,
        IFluentEmail fluentEmail)
    {
        _logger = logger;
        _emailQueueRepository = emailQueueRepository;
        _fluentEmail = fluentEmail;
        _config = ServicesConfigContainer.Config;
    }

    public async Task QueueNewEmail(QueuedEmail queuedEmail)
    {
        if (!_config.Smtp.Enable)
            throw new EmailNotAvailableApiException();
        
        await _emailQueueRepository.Create(queuedEmail);
    }

    public async Task<List<QueuedEmail>> GetQueuedEmails(int limit = 1000)
    {
        var emailQueue = await _emailQueueRepository.GetNextQueuedEmails(limit);
        return emailQueue.ToList();
    }
    
    public async Task SendEmails()
    {
        var emailsToSend = await GetQueuedEmails();
        foreach (var email in emailsToSend)
        {
            
        }
    }

}