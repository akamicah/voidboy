using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Services;
using DirectoryService.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/accounts")]
[ApiController]
public sealed class AccountsController : V1ApiController
{
    private UserActivationService _userActivationService;

    public AccountsController(UserActivationService userActivationService)
    {
        _userActivationService = userActivationService;
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
    [HttpGet("{accountId:guid}")]
    [Authorise]
    public async Task<IActionResult> GetAccount(Guid accountId)
    {
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
    [HttpDelete("{accountId:guid}")]
    [Authorise(UserRole.Admin)]
    public async Task<IActionResult> DeleteAccount(Guid accountId)
    {
        //TODO
        throw new NotImplementedException();
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
    [HttpGet("{accountId:guid}/tokens")]
    [Authorise]
    public async Task<IActionResult> GetAccountTokens(Guid accountId)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Delete an account's token
    /// </summary>
    [HttpDelete("{accountId:guid}/tokens/{token:guid}")]
    [Authorise]
    public async Task<IActionResult> DeleteAccountToken(Guid accountId, Guid token)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Email verification endpoint
    /// </summary>
    [HttpGet("verify/email")]
    [AllowAnonymous]
    public async Task<IActionResult> EmailVerificationEndpoint([FromQuery] EmailVerificationModel verification)
    {
        if (!Guid.TryParse(verification.AccountId, out var accountId))
            return Failure();
        
        if (!Guid.TryParse(verification.VerificationToken, out var verificationToken))
            return Failure();

        await _userActivationService.ReceiveUserActivationResponse(accountId, verificationToken);

        //TODO Redirect to configured route
        return new RedirectResult("https://overte.org");
    }

    public class EmailVerificationModel
    {
        [FromQuery(Name = "a")]
        public string? AccountId { get; set; }
        
        [FromQuery(Name = "v")]
        public string? VerificationToken { get; set; }
    }

}