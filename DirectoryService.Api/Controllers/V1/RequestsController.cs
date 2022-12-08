using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/requests")]
[ApiController]
public sealed class RequestsController : V1ApiController
{
    [HttpGet]
    [Authorise]
    public async Task<IActionResult> GetRequests()
    {
        //TODO
        throw new NotImplementedException();
    }
}