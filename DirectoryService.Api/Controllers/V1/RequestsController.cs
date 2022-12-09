using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/requests")]
[ApiController]
public sealed class RequestsController : V1ApiController
{
    /// <summary>
    /// Pull a list of requests
    /// </summary>
    [HttpGet]
    [Authorise]
    public async Task<IActionResult> GetRequests()
    {
        //TODO Investigate exactly what is supposed to be returned here
        throw new NotImplementedException();
    }
}