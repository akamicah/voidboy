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
}