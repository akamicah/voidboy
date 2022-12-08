using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/places")]
[ApiController]
public sealed class PlacesController : V1ApiController
{
    /// <summary>
    /// Retrieve a list of places
    /// </summary>
    [HttpGet]
    [Authorise]
    public async Task<IActionResult> GetPlaces()
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// Register a new place
    /// </summary>
    [HttpPost]
    [Authorise]
    public async Task<IActionResult> AddPlace()
    {
        //TODO
        throw new NotImplementedException();
    }

    [HttpDelete("{placeId:guid}")]
    [Authorise]
    public async Task<IActionResult> DeletePlace(Guid placeId)
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// Update place information
    /// </summary>
    [HttpPut("{placeId:guid")]
    [Authorise]
    public async Task<IActionResult> UpdatePlaceInfo(Guid placeId)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    
    
    
    
}