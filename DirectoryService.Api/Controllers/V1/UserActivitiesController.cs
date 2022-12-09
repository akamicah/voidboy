using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/user_activities")]
[ApiController]
public sealed class UserActivitiesController : V1ApiController
{
    /// <summary>
    /// Log User Activity 
    /// </summary>
    // TODO: Audit what gets logged and if it should be logged
    [HttpPost]
    [Authorise]
    public async Task<IActionResult> PostUserActivity()
    {
        //TODO
        throw new NotImplementedException();
    }
}