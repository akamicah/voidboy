using DirectoryService.Core.Dto;
using DirectoryService.Core.Entities;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Core.Services;

// ReSharper disable once ClassNeverInstantiated.Global

[ScopedDependency]
public sealed class OAuthService
{
    private readonly ILogger<OAuthService> _logger;
    private readonly UserService _userService;
    private readonly ISessionTokenRepository _sessionTokenRepository;
    private readonly ServiceConfiguration _configuration;

    public OAuthService(ILogger<OAuthService> logger,
        UserService userService,
        ISessionTokenRepository sessionTokenRepository)
    {
        _logger = logger;
        _userService = userService;
        _sessionTokenRepository = sessionTokenRepository;
        _configuration = ServicesConfigContainer.Config;
    }
    
    /// <summary>
    /// Handle Token Grant Request
    /// </summary>
    public async Task<GrantedTokenDto> HandleGrantRequest(TokenGrantRequestDto request)
    {
        if(request.Username is null or "" || request.Password is null or "")
            throw new ArgumentException("Username/Password cannot be empty");
        
        if(request.GrantType is null)
            throw new ArgumentException("Grant type cannot be empty");
        
        switch (request.GrantType.ToAuthGrantType())
        {
            case OAuthGrantType.Password:
                return await GrantTokenFromPassword(request.Username, request.Password, TokenScope.Owner);
            
            case OAuthGrantType.RefreshToken:
                if (request.RefreshToken is null or "")
                    throw new ArgumentException("Refresh token cannot be empty");

                if (!Guid.TryParse(request.RefreshToken, out var refreshToken))
                    throw new InvalidCredentialsApiException();

                return await GrantTokenFromRefreshToken(refreshToken);

            case OAuthGrantType.Invalid:
            case OAuthGrantType.AuthorisationCode:
            default:
                _logger.LogError("Unhandled OAuth2 Grant Type {Type}", request.GrantType);
                throw new ArgumentException("Unknown grant type");
        }
    }

    /// <summary>
    /// Request a session token from a username and password
    /// </summary>
    private async Task<GrantedTokenDto> GrantTokenFromPassword(string username, string password, TokenScope scope)
    {
        var user = await _userService.AuthenticateUser(username, password);
        _logger.LogInformation("Granting token from username/password for user: {Username}", user.Username);

        var newSession = new SessionToken()
        {
            UserId = user.Id,
            Expires = scope switch
            {
                TokenScope.Owner => DateTime.Now.Add(new TimeSpan(0, _configuration.Tokens.OwnerTokenLifetimeHours, 0,
                    0)),
                TokenScope.Domain => DateTime.Now.Add(new TimeSpan(0, _configuration.Tokens.DomainTokenLifetimeHours,
                    0, 0)),
                _ => throw new ArgumentOutOfRangeException(nameof(scope), scope, null)
            },
            Scope = scope
        };
        
        var token = await _sessionTokenRepository.Create(newSession);
        var response = new GrantedTokenDto()
        {
            TokenType = "Bearer",
            AccessToken = token!.Id.ToString(),
            RefreshToken = token.RefreshToken.ToString(),
            ExpiresIn = ((DateTimeOffset)token.Expires).ToUnixTimeSeconds(),
            CreatedAt = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds(),
            AccountId = user.Id.ToString(),
            AccountName = user.Username!,
            Scope = TokenScope.Owner.ToScopeString(),
            AccountRoles = new List<string>()
            {
                user.Role.ToRoleString()
            }
        };
        return response;
    }

    /// <summary>
    /// Request new session token from refresh token
    /// </summary>
    private async Task<GrantedTokenDto> GrantTokenFromRefreshToken(Guid refreshToken)
    {
        var refToken = await _sessionTokenRepository.FindByRefreshToken(refreshToken);
        if (refToken == null)
            throw new InvalidTokenApiException();

        var userId = refToken.UserId;
        var user = await _userService.FindById(userId);
        
        if(user == null)
            throw new InvalidTokenApiException();

        _logger.LogInformation("Granting token from refresh token for user: {Username}", user.Username);
        
        var newSession = new SessionToken()
        {
            UserId = user.Id,
            Expires = refToken.Scope switch
            {
                TokenScope.Owner => DateTime.Now.Add(new TimeSpan(0, _configuration.Tokens.OwnerTokenLifetimeHours, 0,
                    0)),
                TokenScope.Domain => DateTime.Now.Add(new TimeSpan(0, _configuration.Tokens.DomainTokenLifetimeHours,
                    0, 0)),
                _ => throw new ArgumentOutOfRangeException(nameof(refToken.Scope), refToken.Scope, null)
            }
        };
        
        //TODO Should we be deleting the old token?
        var token = await _sessionTokenRepository.Create(newSession);
        var response = new GrantedTokenDto()
        {
            TokenType = "Bearer",
            AccessToken = token!.Id.ToString(),
            RefreshToken = token.RefreshToken.ToString(),
            ExpiresIn = ((DateTimeOffset)token.Expires).ToUnixTimeSeconds(),
            CreatedAt = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds(),
            AccountId = user.Id.ToString(),
            AccountName = user.Username!,
            Scope = TokenScope.Owner.ToScopeString(),
            AccountRoles = new List<string>()
            {
                user.Role.ToRoleString()
            }
        };
        return response;
    }
}