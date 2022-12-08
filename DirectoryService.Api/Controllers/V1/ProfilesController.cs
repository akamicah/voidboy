using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/profiles")]
[ApiController]
public sealed class ProfilesController : V1ApiController
{
    [HttpGet]
    [Authorise]
    public async Task<IActionResult> GetProfiles()
    {
        //TODO
        throw new NotImplementedException();
    }
}