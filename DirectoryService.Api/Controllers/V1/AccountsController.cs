using System.Text.Json.Serialization;
using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Entities;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.Services;
using DirectoryService.Shared;
using DirectoryService.Shared.Config;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/accounts")]
[ApiController]
public sealed class AccountsController : V1ApiController
{
    private readonly ServiceConfiguration _configuration;
    private readonly UserActivationService _userActivationService;
    private readonly SessionTokenService _sessionTokenService;
    private readonly UserService _userService;
    
    public AccountsController(UserActivationService userActivationService,
        SessionTokenService sessionTokenService,
        UserService userService)
    {
        _configuration = ServicesConfigContainer.Config;
        _userActivationService = userActivationService;
        _sessionTokenService = sessionTokenService;
        _userService = userService;
    }
    
    /// <summary>
    /// Fetch a list of accounts
    /// </summary>
    [HttpGet]
    [Authorise]
    public async Task<IActionResult> GetAccounts()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Get account by account id
    /// </summary>
    [HttpGet("{accountReference}")]
    [Authorise]
    public async Task<IActionResult> GetAccount(string accountReference)
    {
        var account = await _userService.FindUser(accountReference);
        if (account is null)
            throw new UserNotFoundApiException();
        
        RestrictToSelfOrAdmin(account.Id);
        
        
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Update account
    /// </summary>
    [HttpPost("{accountId:guid}")]
    [Authorise]
    public async Task<IActionResult> UpdateAccount(Guid accountId, [FromBody] UpdateAccountDto updateAccount)
    {
        updateAccount.AccountId = accountId;
        
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// (Admin) Delete account by account id
    /// </summary>
    [HttpDelete("{accountReference}")]
    [Authorise(UserRole.Admin)]
    public async Task<IActionResult> DeleteAccount(string accountReference)
    {
        var account = await _userService.FindUser(accountReference);
        if (account is null)
            throw new UserNotFoundApiException();
        
        await _userService.DeleteUser(account.Id);
        return Success();
    }
    
    /// <summary>
    /// Fetch specific account property by field name
    /// </summary>
    // Is this in use?
    [HttpGet("{accountId:guid}/field/{fieldName}")]
    [Authorise]
    public async Task<IActionResult> GetAccountField(Guid accountId, string fieldName)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Update account property by field name
    /// </summary>
    // Is this in use?
    [HttpPost("{accountId:guid}/field/{fieldName}")]
    [Authorise]
    public async Task<IActionResult> SetAccountField(Guid accountId, string fieldName, [FromBody] UpdateFieldDto fieldUpdate)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Return active token(s) for account
    /// </summary>
    [HttpGet("{accountReference}/tokens")]
    [Authorise]
    public async Task<IActionResult> GetAccountTokens(string accountReference)
    {
        var account = await _userService.FindUser(accountReference);
        if (account is null)
            throw new UserNotFoundApiException();
        
        RestrictToSelfOrAdmin(account.Id);
        
        var page = PaginatedRequest();

        var result = await _sessionTokenService.ListAccountTokens(account.Id, page);
        return new JsonResult(result.Data is not null ? new AccountTokenListModel(result.Data) : new AccountTokenListModel());
    }

    /// <summary>
    /// Model for V1 of the API
    /// </summary>
    private class AccountTokenModel
    {
        [JsonPropertyName("id")] 
        public Guid Id { get; set; }
        
        [JsonPropertyName("tokenid")]
        public Guid TokenId { get; set; }
        
        [JsonPropertyName("token")]
        public Guid Token { get; set; }
        
        [JsonPropertyName("refresh_token")]
        public Guid RefreshToken { get; set; }
        
        [JsonPropertyName("token_creation_time")]
        public DateTime CreatedOn { get; set; }
        
        [JsonPropertyName("token_expiration_time")]
        public DateTime ExpiresOn { get; set; }
        
        [JsonPropertyName("scope")]
        public IEnumerable<string>? Scope { get; set; }
    }
    
    /// <summary>
    /// Model for V1 of the API
    /// </summary>
    private class AccountTokenListModel
    {
        public AccountTokenListModel()
        {
            Tokens = new List<AccountTokenModel>();
        }

        public AccountTokenListModel(IEnumerable<SessionToken> sessionTokens)
        {
            Tokens = new List<AccountTokenModel>();
            foreach (var sessionToken in sessionTokens)
            {
                Tokens.Add(new AccountTokenModel()
                {
                    CreatedOn = sessionToken.CreatedAt,
                    ExpiresOn = sessionToken.Expires,
                    Id = sessionToken.Id,
                    RefreshToken = sessionToken.RefreshToken,
                    Token = sessionToken.Id,
                    TokenId = sessionToken.Id,
                    Scope = new []
                    {
                        sessionToken.Scope.ToScopeString()
                    }
                });
            }
        }
        
        [JsonPropertyName("tokens")]
        public List<AccountTokenModel> Tokens { get; set; }
    }
    
    /// <summary>
    /// Delete an account's token
    /// </summary>
    [HttpDelete("{accountReference}/tokens/{token:guid}")]
    [Authorise]
    public async Task<IActionResult> DeleteAccountToken(string accountReference, Guid token)
    {
        var account = await _userService.FindUser(accountReference);
        if (account is null)
            throw new UserNotFoundApiException();
        
        RestrictToSelfOrAdmin(account.Id);
        
        await _sessionTokenService.RevokeAccountToken(account.Id, token);
        
        return Success();
    }
    
    /// <summary>
    /// Email verification endpoint
    /// </summary>
    [HttpGet("verify/email")]
    [AllowAnonymous]
    public async Task<IActionResult> EmailVerificationEndpoint([FromQuery] EmailVerificationModel verification)
    {
        if (!Guid.TryParse(verification.AccountId, out var accountId))
            return new RedirectResult(_configuration.Registration.EmailVerificationFailRedirect!);
        
        if (!Guid.TryParse(verification.VerificationToken, out var verificationToken))
            return new RedirectResult(_configuration.Registration.EmailVerificationFailRedirect!);

        try
        {
            await _userActivationService.ReceiveUserActivationResponse(accountId, verificationToken);
            return new RedirectResult(_configuration.Registration.EmailVerificationSuccessRedirect!);
        }
        catch (Exception e)
        {
            return new RedirectResult(_configuration.Registration.EmailVerificationFailRedirect!);
        }
    }

    /// <summary>
    /// Model for V1 of the API
    /// </summary>
    public class EmailVerificationModel
    {
        [FromQuery(Name = "a")]
        public string? AccountId { get; set; }
        
        [FromQuery(Name = "v")]
        public string? VerificationToken { get; set; }
    }

}