using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/places")]
[ApiController]
public sealed class PlacesController : V1ApiController
{
    [HttpGet]
    [Authorise]
    public async Task<IActionResult> GetPlaces()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Authorise]
    public async Task<IActionResult> AddPlace()
    {
        //TODO
        throw new NotImplementedException();
    }
}