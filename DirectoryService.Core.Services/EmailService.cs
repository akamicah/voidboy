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

[ScopedDependency]
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

    /// <summary>
    /// Add a new e-mail to the queue
    /// </summary>
    public async Task QueueNewEmail(QueuedEmail queuedEmail)
    {
        if (!_config.Smtp.Enable)
            throw new EmailNotAvailableApiException();
        
        await _emailQueueEntityRepository.Create(queuedEmail);
    }

    /// <summary>
    /// Return a list of queued emails
    /// </summary>
    public async Task<List<QueuedEmail>> GetQueuedEmails(int limit = 1000)
    {
        var emailQueue = await _emailQueueEntityRepository.GetNextQueuedEmails(limit);
        return emailQueue.ToList();
    }

    /// <summary>
    /// Clear sent emails where send date is over 30 days ago
    /// </summary>
    public async Task ClearSentEmails()
    {
        await _emailQueueEntityRepository.ClearSentEmails(DateTime.Now.AddDays(-30));
    }

    /// <summary>
    /// Re-try the email 3 times before deleting
    /// </summary>
    private async Task ReQueueForRetry(QueuedEmail email)
    {
        if (email.Attempt == 3)
        {
            _logger.LogInformation("Email {id} failed to send 3 times. Deleting.", email.Id);
            await _emailQueueEntityRepository.Delete(email.Id);
            return;
        }
        _logger.LogInformation("Re-queueing email {id} for sending in 30 seconds", email.Id);
        email.Attempt += 1;
        email.SendOn = email.SendOn.AddSeconds(30);
        await _emailQueueEntityRepository.Update(email);
    }

    /// <summary>
    /// Send the email and mark as sent if successful, or queue for retry otherwise
    /// </summary>
    private async Task SendEmail(QueuedEmail email)
    {
        var user = await _userRepository.Retrieve(email.AccountId);
        if (user != null)
        {
            var template = email.Type switch
            {
                EmailType.ActivationEmail => _templateDirectory + "activationEmail.en.html",
                _ => ""
            };

            if (template == "")
                throw new ArgumentOutOfRangeException(email.Type.ToString());

            var model = new EmailModel();
            if (email.Model != null)
                model = JsonConvert.DeserializeObject<EmailModel>(email.Model);
            
            var mail = _fluentEmail.To(user.Email, user.Username)
                .Subject(model.Subject)
                .SetFrom(_config.Smtp.SenderEmail, _config.Smtp.SenderName)
                .UsingTemplateFromFile(template, model, true);
            var response = await mail.SendAsync();
            if (response.Successful)
            {
                email.Sent = true;
                email.SentOn = DateTime.Now;
                await _emailQueueEntityRepository.Update(email);
            }
            else
            {
                _logger.LogError("Failed to send email {id}: {errors}", email.Id, string.Join(", ", response.ErrorMessages));
                await ReQueueForRetry(email);
            }
        }
    }

    /// <summary>
    /// This model must contain all possible variables an email might contain
    /// </summary>
    private class EmailModel
    {
        public string? Subject { get; set; }
        public string? Username { get; set; }
        public string? ActivationUrl { get; set; }
        public string? Timeout { get; set; }
    }
    
    /// <summary>
    /// Process the email queue
    /// </summary>
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
                await ReQueueForRetry(email);
            }
        }
    }
}