using DirectoryService.Core.Entities;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using Newtonsoft.Json;
using Humanizer;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Core.Services;

[ScopedDependency]
public class UserActivationService
{
    private readonly ILogger<UserActivationService> _logger;
    private readonly IEmailService _emailService;
    private readonly IActivationTokenRepository _activationTokenRepository;
    private readonly ServiceConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public UserActivationService(ILogger<UserActivationService> logger,
        IEmailService emailService,
        IActivationTokenRepository activationTokenRepository,
        IUserRepository userRepository)
    {
        _logger = logger;
        _emailService = emailService;
        _activationTokenRepository = activationTokenRepository;
        _configuration = ServiceConfigurationContainer.Config;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Process the activation token request
    /// </summary>
    public async Task ReceiveUserActivationResponse(Guid accountId, Guid verificationToken)
    {
        var token = await _activationTokenRepository.Retrieve(verificationToken);
        if (token is null || token.UserId != accountId) throw new InvalidTokenApiException();
        
        if(token.UserId != accountId) throw new InvalidTokenApiException();

        var user = await _userRepository.Retrieve(accountId);
        
        if(user == null) throw new InvalidTokenApiException();

        _logger.LogInformation("Activating user: {username}", user.Username);
        
        user.Activated = true;
        await _userRepository.Update(user);
        await _activationTokenRepository.Delete(token.Id);
    }
    
    /// <summary>
    /// Create activation token and send an activation email
    /// </summary>
    public async Task SendUserActivationRequest(User user)
    {
        if (_configuration.Registration.RequireEmailVerification)
        {
            var activationToken = await _activationTokenRepository.Create(new ActivationToken()
            {
                UserId = user.Id,
                Expires = DateTime.Now.AddMinutes(_configuration.Registration.EmailVerificationTimeoutMinutes)
            });

            var timeout = TimeSpan.FromMinutes(_configuration.Registration.EmailVerificationTimeoutMinutes);
            
            var email = new QueuedEmail()
            {
                Type = EmailType.ActivationEmail,
                UserId = user.Id,
                Model = JsonConvert.SerializeObject(new 
                {
                    Subject = "Activate your Account",
                    user.Username,
                    ActivationUrl = _configuration.MetaverseInfo.ServerUrl + "/api/v1/accounts/verify/email?a=" + user.Id + "&v=" + activationToken.Id,
                    Timeout = timeout.Humanize()
                }),
                SendOn = DateTime.Now
            };

            await _emailService.QueueNewEmail(email);
        }
    }
}