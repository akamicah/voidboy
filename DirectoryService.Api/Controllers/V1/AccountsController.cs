using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using DirectoryService.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/accounts")]
[ApiController]
public sealed class AccountsController : V1ApiController
{
    [HttpGet]
    [Authorise]
    public async Task<IActionResult> GetAccounts()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    [HttpGet("{accountId:guid}")]
    [Authorise]
    public async Task<IActionResult> GetAccount(Guid accountId)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    [HttpPost("{accountId:guid}")]
    [Authorise]
    public async Task<IActionResult> UpdateAccount(Guid accountId, [FromBody] UpdateAccountDto updateAccount)
    {

        updateAccount.AccountId = accountId;
        
        //TODO
        throw new NotImplementedException();
    }
    
    [HttpDelete("{accountId:guid}")]
    [Authorise(UserRole.Admin)]
    public async Task<IActionResult> DeleteAccount(Guid accountId)
    {
        //TODO
        throw new NotImplementedException();
    }
}