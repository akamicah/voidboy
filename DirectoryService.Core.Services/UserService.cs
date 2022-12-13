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

[ScopedDependency]
public sealed class UserService
{
    private readonly ILogger<UserService> _logger;
    private readonly RegisterUserValidator _registerUserValidator;
    private readonly IUserRepository _userRepository;
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly ServiceConfiguration _configuration;
    private readonly ISessionProvider _sessionProvider;
    private readonly UserActivationService _userActivationService;
    private readonly IUserGroupRepository _userGroupRepository;

    public UserService(ILogger<UserService> logger,
        RegisterUserValidator registerUserValidator,
        IUserRepository userRepository,
        ISessionProvider sessionProvider,
        UserActivationService userActivationService,
        IUserProfileRepository userProfileRepository,
        IUserGroupRepository userGroupRepository)
    {
        _logger = logger;
        _registerUserValidator = registerUserValidator;
        _userRepository = userRepository;
        _configuration = ServicesConfigContainer.Config;
        _sessionProvider = sessionProvider;
        _userActivationService = userActivationService;
        _userProfileRepository = userProfileRepository;
        _userGroupRepository = userGroupRepository;
    }

    /// <summary>
    /// Get user by Id
    /// </summary>
    public async Task<User?> FindById(Guid id)
    {
        return await _userRepository.Retrieve(id);
    }

    /// <summary>
    /// Find a user either by user/account Id or by username.
    /// </summary>
    /// <param name="needle">User Id, Username</param>
    /// <returns>Either the user entity or null</returns>
    public async Task<User?> FindUser(string needle)
    {
        if (Guid.TryParse(needle, out var userId))
        {
            return await _userRepository.Retrieve(userId);
        }
        return await _userRepository.FindByUsername(needle.ToLower());
    }
    
    /// <summary>
    /// Fetch a list of users relative to the requester session
    /// </summary>
    public async Task<PaginatedResult<UserSearchResultDto>> ListRelativeUsers(PaginatedRequest page)
    {
        if(!page.AsAdmin || (page.Filter != null && page.Filter.Contains("connections")))
            page.Where.Add("connection", true);
        
        if(page.Filter != null && page.Filter.Contains("friends"))
            page.Where.Add("friend", true);
        
        var requestedBy = await _sessionProvider.GetRequesterSession();
        var users = await _userRepository.ListRelativeUsers(requestedBy!.AccountId, page, true);
        var result = new List<UserSearchResultDto>();
        foreach (var user in users.Data!)
        {
            var profile = await _userProfileRepository.Retrieve(user.Id);
            result.Add(new UserSearchResultDto()
            {
                Username = user.Username,
                AccountId = user.Id,
                Images = new UserImagesDto()
                {
                    Hero = profile?.HeroImageUrl ?? "",
                    Thumbnail = profile?.ThumbnailImageUrl ?? "",
                    Tiny = profile?.TinyImageUrl ?? ""
                },
                Location = new LocationReferenceDto()
                {
                    NodeId = Guid.Empty,
                    Online = false,
                    Path = "",
                    Root = new RootLocationReferenceDto()
                    {
                        Domain = new DomainReferenceDto()
                        {
                            Id = Guid.Empty,
                            IceServerAddress = "",
                            Name = "",
                            NetworkAddress = "",
                            NetworkPort = 0
                        }
                    }
                }
            });
        }

        return new PaginatedResult<UserSearchResultDto>(result, users);
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    public async Task<UserRegisteredDto> RegisterUser(RegisterUserDto registerUserDto)
    {
        await _registerUserValidator.ValidateAndThrowAsync(registerUserDto);

        // TODO: Should really send a password reset email to prevent registered user email harvesting
        if (await _userRepository.FindByEmail(registerUserDto.Email!.ToLower()) != null)
            throw new EmailAddressTakenApiException();

        if (await _userRepository.FindByUsername(registerUserDto.Username!.ToLower()) != null)
            throw new UsernameTakenApiException();
        
        var auth = CryptographyService.GenerateAuth(registerUserDto.Password!);

        var role = UserRole.User;

        if (_configuration.Registration.DefaultAdminAccount != "" &&
            registerUserDto.Username == _configuration.Registration.DefaultAdminAccount)
        {
            _logger.LogWarning("Default administrator account being registered: {username}", registerUserDto.Username);
            role = UserRole.Admin;
        }

        var newUser = new User()
        {
            Username = registerUserDto.Username!,
            Email = registerUserDto.Email!.ToLowerInvariant(),
            Activated = !_configuration.Registration.RequireEmailVerification,
            AuthVersion = auth.Version,
            AuthHash = auth.Hash,
            Role = role,
            CreatorIp = registerUserDto.OriginIp?.ToString() ?? IPAddress.Any.ToString(),
            State = AccountState.Normal
        };

        var createdUser = await _userRepository.Create(newUser);

       // Create default profile
        await _userProfileRepository.Create(new UserProfile()
        {
            UserId = createdUser.Id,
            HeroImageUrl = "",
            ThumbnailImageUrl = "",
            TinyImageUrl = ""
        });
        
        // Create default connection and friend user groups
        var connectionsGroup = await _userGroupRepository.Create(new UserGroup()
        {
            Name = "",
            Description = registerUserDto.Username + "'s Connections",
            Internal = true,
            Rating = MaturityRating.Everyone,
            OwnerUserId = createdUser.Id
        });

        var friendsGroup = await _userGroupRepository.Create(new UserGroup()
        {
            Name = "",
            Description = registerUserDto.Username + "'s Friends",
            Internal = true,
            Rating = MaturityRating.Everyone,
            OwnerUserId = createdUser.Id
        });

        createdUser.ConnectionGroup = connectionsGroup.Id;
        createdUser.FriendsGroup = friendsGroup.Id;

        await _userRepository.Update(createdUser);

        _logger.LogInformation("New user registration. Username: {username}, Email: {email}",
            registerUserDto.Username, registerUserDto.Email!.MaskEmail());

        if (_configuration.Registration.RequireEmailVerification)
            await _userActivationService.SendUserActivationRequest(createdUser!);
        
        return new UserRegisteredDto
        {
            AccountId = createdUser!.Id.ToString(),
            Username = createdUser.Username,
            AccountAwaitingVerification = _configuration.Registration.RequireEmailVerification,
            AccountIsActive = createdUser.Activated
        };
    }
    
    /// <summary>
    /// Attempt to login user
    /// </summary>
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

        if (user.Activated) return user;
        
        await _userActivationService.SendUserActivationRequest(user);
        throw new UserNotVerifiedApiException();
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    public async Task DeleteUser(Guid userId)
    {
        var user = await _userRepository.Retrieve(userId);
        if (user is null)
            throw new UserNotFoundApiException();

        _logger.LogInformation("Deleting user {username} - User ID: {uid}", user.Username, user.Id);
        await _userRepository.Delete(user.Id);
    }
}