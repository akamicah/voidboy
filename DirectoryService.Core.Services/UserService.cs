using System.Net;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Entities;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Core.Validators;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using DirectoryService.Shared.Extensions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Core.Services;

// ReSharper disable once ClassNeverInstantiated.Global

[ScopedRegistration]
public sealed class UserService
{
    private readonly ILogger<UserService> _logger;
    private readonly RegisterUserValidator _registerUserValidator;
    private readonly IUserRepository _userRepository;
    private readonly ServiceConfiguration _configuration;
    private readonly ISessionProvider _sessionProvider;
    private readonly UserActivationService _userActivationService;

    public UserService(ILogger<UserService> logger,
        RegisterUserValidator registerUserValidator,
        IUserRepository userRepository,
        ISessionProvider sessionProvider,
        UserActivationService userActivationService)
    {
        _logger = logger;
        _registerUserValidator = registerUserValidator;
        _userRepository = userRepository;
        _configuration = ServicesConfigContainer.Config;
        _sessionProvider = sessionProvider;
        _userActivationService = userActivationService;
    }

    public async Task<PaginatedResponse<User>> ListUsers(PaginatedRequest page)
    {
        var requester = await _sessionProvider.GetRequesterSession();
        if (page.IsAdmin && requester!.Role != UserRole.Admin)
            throw new UnauthorisedApiException();

        // TODO: Figure out what we're returning and make sure it's not excessive information
        
        throw new NotImplementedException();
    }

    public async Task<UserRegisteredDto> RegisterUser(RegisterUserDto registerUserDto)
    {
        await _registerUserValidator.ValidateAndThrowAsync(registerUserDto);

        // TODO: Should really send a password reset email to prevent registered user email harvesting
        if (await _userRepository.FindByEmail(registerUserDto.Email!.ToLower()) != null)
            throw new EmailAddressTakenApiException();

        if (await _userRepository.FindByUsername(registerUserDto.Username!.ToLower()) != null)
            throw new UsernameTakenApiException();
        
        var auth = CryptographyService.GenerateAuth(registerUserDto.Password!);

        var newUser = new User()
        {
            Username = registerUserDto.Username!,
            Email = registerUserDto.Email!.ToLowerInvariant(),
            Activated = !_configuration.Registration.RequireEmailVerification,
            AuthVersion = auth.Version,
            AuthHash = auth.Hash,
            Role = UserRole.User,
            CreatorIp = registerUserDto.OriginIp?.ToString() ?? IPAddress.Any.ToString()
        };

        var response = await _userRepository.Create(newUser);

        _logger.LogInformation("New user registration. Username: {username}, Email: {email}",
            registerUserDto.Username, registerUserDto.Email!.MaskEmail());

        if (_configuration.Registration.RequireEmailVerification)
            await _userActivationService.SendUserActivationRequest(response!);
        
        return new UserRegisteredDto
        {
            AccountId = response!.Id.ToString(),
            Username = response.Username,
            AccountAwaitingVerification = _configuration.Registration.RequireEmailVerification,
            AccountIsActive = response.Activated
        };
    }
    
    public async Task<User> AuthenticateUser(string? username, string? password)
    {
        if(username == null || password == null)
            throw new InvalidCredentialsApiException();

        // Allow user to login with either username or email address
        var user = await _userRepository.FindByUsername(username.ToLowerInvariant());
        if (user == null)
        {
            user = await _userRepository.FindByEmail(username.ToLowerInvariant());
                if(user == null)
                    throw new InvalidCredentialsApiException();
        }

        if(!CryptographyService.AuthenticatePassword(password, user.AuthHash!))
            throw new InvalidCredentialsApiException();

        if (!user.Activated)
            throw new UserNotVerifiedApiException();

        return user;
    }

    public async Task<User?> GetUserFromId(Guid id)
    {
        return await _userRepository.Retrieve(id);
    }
}