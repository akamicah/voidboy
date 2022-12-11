using System.Linq.Expressions;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Entities;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using FluentEmail.Core;
using FluentEmail.Liquid;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DirectoryService.Core.Services;

[ScopedRegistration]
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IEmailQueueEntityRepository _emailQueueEntityRepository;
    private readonly IFluentEmail _fluentEmail;
    private readonly ServiceConfiguration _config;
    private readonly IUserRepository _userRepository;
    private readonly string _templateDirectory;
    
    public EmailService(ILogger<EmailService> logger,
        IEmailQueueEntityRepository emailQueueEntityRepository,
        IFluentEmail fluentEmail,
        IUserRepository userRepository)
    {
        _logger = logger;
        _emailQueueEntityRepository = emailQueueEntityRepository;
        _fluentEmail = fluentEmail;
        _config = ServicesConfigContainer.Config;
        _userRepository = userRepository;
        
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        _templateDirectory = Path.Combine(Path.GetDirectoryName(assembly.Location)!, "templates/email/");
        
    }

    public async Task QueueNewEmail(QueuedEmail queuedEmail)
    {
        if (!_config.Smtp.Enable)
            throw new EmailNotAvailableApiException();
        
        await _emailQueueEntityRepository.Create(queuedEmail);
    }

    public async Task<List<QueuedEmail>> GetQueuedEmails(int limit = 1000)
    {
        var emailQueue = await _emailQueueEntityRepository.GetNextQueuedEmails(limit);
        return emailQueue.ToList();
    }

    private async Task SendActivationEmail(User user, QueuedEmail email)
    {
        await _fluentEmail.To(user.Email)
            .Subject("Activate your Account")
            .UsingTemplateFromFile( _templateDirectory + "activationEmail.liquid", new 
            {
                user.Username,
                ActivationLink = "Test123",
                ExpireTime = "Now!"
            }, true).SendAsync();
    }

    private async Task SendEmail(QueuedEmail email)
    {
        var user = await _userRepository.Retrieve(email.AccountId);
        if (user != null)
        {
            switch (email.Type)
            {
                case EmailType.ActivationEmail:
                    await SendActivationEmail(user, email);
                    break;

                case EmailType.Invalid:
                default:
                    throw new ArgumentOutOfRangeException(email.Type.ToString());
            }
        }
    }
    
    public async Task SendEmails()
    {
        var emailsToSend = await GetQueuedEmails();
        if (emailsToSend.Count == 0)
            return;
        
        _logger.LogInformation("Sending {count} emails from queue.", emailsToSend.Count);
        
        foreach (var email in emailsToSend)
        {
            try
            {
                await SendEmail(email);
            }
            catch (Exception e)
            {
                _logger.LogError("Error sending email {id}. {exception}", email.Id, e.Message);
                //TODO: Increment attempt, and if attempt = 3 delete
                //await _emailQueueEntityRepository.Delete(email);
            }
        }
    }

}