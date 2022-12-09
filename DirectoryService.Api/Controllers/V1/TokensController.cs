using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/tokens")]
[ApiController]
public sealed class TokensController : V1ApiController
{
    [HttpGet]
    [Authorise]
    public async Task<IActionResult> GetTokens()
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// Authorise a new token for the logged in account
    /// </summary>
    [HttpPost("/api/v1/token/new")]
    [Authorise]
    public async Task<IActionResult> CreateToken()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Create a new token or redirect to login
    /// </summary>
    // TODO: Is this even necessary anymore?
    [HttpGet("/token/new")]
    [HttpGet("/user/token/new")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateTokenOrRedirect()
    {
        //TODO
        throw new NotImplementedException();
    }
}