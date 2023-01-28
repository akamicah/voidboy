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
    private readonly UserGroupService _userGroupService;
    private readonly UserPresenceService _userPresenceService;
    private readonly IDomainRepository _domainRepository;
    private readonly CryptographyService _cryptographyService;

    public UserService(ILogger<UserService> logger,
        RegisterUserValidator registerUserValidator,
        IUserRepository userRepository,
        ISessionProvider sessionProvider,
        UserActivationService userActivationService,
        IUserProfileRepository userProfileRepository,
        UserGroupService userGroupService,
        UserPresenceService userPresenceService,
        IDomainRepository domainRepository,
        CryptographyService cryptographyService)
    {
        _logger = logger;
        _registerUserValidator = registerUserValidator;
        _userRepository = userRepository;
        _configuration = ServiceConfigurationContainer.Config;
        _sessionProvider = sessionProvider;
        _userActivationService = userActivationService;
        _userProfileRepository = userProfileRepository;
        _userGroupService = userGroupService;
        _userPresenceService = userPresenceService;
        _domainRepository = domainRepository;
        _cryptographyService = cryptographyService;
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

    public async Task<UserInfoDto?> GetUserInfo(Guid userId)
    {
        var user = await _userRepository.Retrieve(userId);
        if (user is null) return null;

        var profile = await _userProfileRepository.Retrieve(userId);
        var presence = await _userPresenceService.GetUserPresence(userId);

        UserLocationDto? location = null; 
        if (presence is not null)
        {
            location = new UserLocationDto()
            {
                Domain = await _domainRepository.Retrieve(presence.DomainId!.Value),
                Path = presence.Path
            };
        }

        var userInfo = new UserInfoDto()
        {
            Id = userId,
            Username = user.Username,
            Administrator = user.Role == UserRole.Admin,
            Availability = new List<string>(),
            CreationDate = user.CreatedAt,
            Email = user.Email,
            Enabled = user.Enabled,
            Role = user.Role,
            Images = new UserImagesDto()
            {
                Hero = profile?.HeroImageUrl,
                Tiny = profile?.TinyImageUrl,
                Thumbnail = profile?.ThumbnailImageUrl
            },
            ProfileDetail = "", //TODO
            PublicKey = presence?.PublicKey,
            Location = location,
            LastHeartbeat = presence?.LastHeartbeat
        };
        var connections = await _userGroupService.GetGroupMembers(user.ConnectionGroup, PaginatedRequest.All());
        var friends = await _userGroupService.GetGroupMembers(user.FriendsGroup, PaginatedRequest.All());
        userInfo.Connections = connections.Data?.ToList();
        userInfo.Friends = friends.Data?.ToList();
        
        return userInfo;
    }

    public async Task<List<string>> ConvertUserIdsToUsernames(List<Guid> userIds)
    {
        return await _userRepository.UserIdsToUsernames(userIds);
    }
    
    /// <summary>
    /// Fetch a list of users relative to the requester session
    /// </summary>
    public async Task<PaginatedResult<UserSearchResultDto>> ListRelativeUsers(PaginatedRequest page)
    {
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null) throw new UnauthorisedApiException();

        if (!session.AsAdmin || (page.Filter != null && page.Filter.Contains("connections")))
            page.Where.Add("connection", true);

        if (page.Filter != null && page.Filter.Contains("friends"))
            page.Where.Add("friend", true);

        var requestedBy = await _sessionProvider.GetRequesterSession();
        var users = await _userRepository.ListRelativeUsers(requestedBy!.UserId, page, true);
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

        if (_configuration.Registration!.DefaultAdminAccount != "" &&
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

        _logger.LogInformation("New user registration. Username: {username}, Email: {email}, Originating IP: {ip}",
            registerUserDto.Username, registerUserDto.Email!.MaskEmail(),
            registerUserDto.OriginIp?.ToString() ?? "Unknown");

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
        if (username == null || password == null)
            throw new InvalidCredentialsApiException();

        // Allow user to login with either username or email address
        var user = await _userRepository.FindByUsername(username.ToLowerInvariant());
        if (user == null)
        {
            user = await _userRepository.FindByEmail(username.ToLowerInvariant());
            if (user == null)
                throw new InvalidCredentialsApiException();
        }

        if (!CryptographyService.AuthenticatePassword(password, user.AuthHash!))
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
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null || !session.AsAdmin) throw new UnauthorisedApiException();

        var user = await _userRepository.Retrieve(userId);
        if (user is null)
            throw new UserNotFoundApiException();

        _logger.LogInformation("Deleting user {username} - User ID: {uid}", user.Username, user.Id);
        await _userRepository.Delete(user.Id);
    }

    /// <summary>
    /// Update heartbeat for user
    /// </summary>
    public async Task UpdatePublicKey(Guid userId, Stream publicKey)
    {
        var user = await _userRepository.Retrieve(userId);
        if (user is null)
            throw new UserNotFoundApiException();

        var presence = await _userPresenceService.GetUserPresence(userId);
        if (presence == null)
        {
            _logger.LogWarning("Set public key for user {username} with no presence.", user.Username);
            presence = new UserPresence()
            {
                Id = userId,
                DomainId = Guid.Empty,
                PlaceId = Guid.Empty,
                NetworkAddress = ""
            };
        }
        presence.PublicKey = _cryptographyService.ConvertPublicKey(publicKey.ToByteArray(), CryptographyService.PublicKeyType.PKCS1_PublicKey);
       
        await _userPresenceService.UpdateUserPresence(presence);
    }

    /// <summary>
    /// Update heartbeat for user by session
    /// </summary>
    public async Task UpdatePublicKey(Stream publicKey)
    {
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null) throw new UnauthorisedApiException();
        
        await UpdatePublicKey(session.UserId, publicKey);
    }

    public async Task ProcessHeartbeat(Guid userId, UserHeartbeatDto userHeartbeat)
    {
        var uid = userHeartbeat.UserId;
    }
    
    public async Task ProcessHeartbeat(UserHeartbeatDto userHeartbeat)
    {
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null) throw new UnauthorisedApiException();

        await ProcessHeartbeat(session.UserId, userHeartbeat);

    }

    public async Task<UserProfileDto> GetUserProfile(Guid userId)
    {
        var user = await _userRepository.Retrieve(userId);
        if (user is null)
            throw new UserNotFoundApiException();

        return new UserProfileDto()
        {
            UserId = userId,
            Username = user.Username
        };
    }
    
    public async Task<UserProfileDto> GetUserProfile()
    {
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null) throw new UnauthorisedApiException();

        return await GetUserProfile(session.UserId);
    }
}