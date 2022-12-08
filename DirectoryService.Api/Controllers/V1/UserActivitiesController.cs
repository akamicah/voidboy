using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/user_activities")]
[ApiController]
public sealed class UserActivitiesController : V1ApiController
{
    [HttpPost]
    [Authorise]
    public async Task<IActionResult> PostUserActivity()
    {
        //TODO
        throw new NotImplementedException();
    }
}