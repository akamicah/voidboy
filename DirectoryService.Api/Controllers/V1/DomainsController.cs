using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/domains")]
[ApiController]
public sealed class DomainsController : V1ApiController
{
    [HttpGet]
    [Authorise]
    public async Task<IActionResult> GetDomains()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Authorise]
    public async Task<IActionResult> AddDomain()
    {
        //TODO
        throw new NotImplementedException();
    }
    
}