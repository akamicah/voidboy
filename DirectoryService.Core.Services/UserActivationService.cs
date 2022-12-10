using DirectoryService.Core.Dto;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using Newtonsoft.Json;

namespace DirectoryService.Core.Services;

[ScopedRegistration]
public class UserActivationService
{
    private readonly IEmailService _emailService;
    private readonly IActivationTokenRepository _activationTokenRepository;
    private readonly ServiceConfiguration _configuration;

    public UserActivationService(IEmailService emailService,
        IActivationTokenRepository activationTokenRepository)
    {
        _emailService = emailService;
        _activationTokenRepository = activationTokenRepository;
        _configuration = ServicesConfigContainer.Config;
    }

    public async Task SendUserActivationRequest(User user)
    {
        if (_configuration.Registration.RequireEmailVerification)
        {
            var activationToken = await _activationTokenRepository.Create(new ActivationToken()
            {
                AccountId = user.Id,
                Expires = DateTime.Now.AddMinutes(_configuration.Registration.EmailVerificationTimeoutMinutes)
            });

            var email = new QueuedEmail()
            {
                Type = EmailType.ActivationEmail,
                AccountId = user.Id,
                Model = JsonConvert.SerializeObject(new 
                {
                    AccountId = user.Id,
                    user.Username,
                    Token = activationToken.Id,
                    Timeout = _configuration.Registration.EmailVerificationTimeoutMinutes
                }),
                SendOn = DateTime.Now
            };

            await _emailService.QueueNewEmail(email);
        }
    }
}