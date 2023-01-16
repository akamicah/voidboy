using AutoMapper;
using DirectoryService.Api.Attributes;
using DirectoryService.Api.Controllers.V1.Models;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Services;
using DirectoryService.Shared;
using Microsoft.AspNetCore.Mvc;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/places")]
[ApiController]
public sealed class PlacesController : V1ApiController
{
    private readonly PlaceService _placeService;
    private readonly IMapper _mapper;

    public PlacesController(PlaceService placeService,
        IMapper mapper)
    {
        _placeService = placeService;
        _mapper = mapper;
    }
    
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
    public async Task<IActionResult> RegisterPlace([FromBodyOrDefault] RegisterPlaceRootModel registerPlaceModel)
    {
        if (registerPlaceModel.Place is null)
            return Failure();

        var place = await _placeService.RegisterNewPlace(_mapper.Map<RegisterPlaceDto>(registerPlaceModel.Place));
        
        //TODO: Return PlaceInfo

        return Success();
    }

    /// <summary>
    /// Delete place
    /// </summary>
    [HttpDelete("{placeId:guid}")]
    [Authorise]
    public async Task<IActionResult> DeletePlace(Guid placeId)
    {
        await _placeService.DeletePlace(placeId);
        return Success();
    }

    /// <summary>
    /// Update place information
    /// </summary>
    [HttpPut("{placeId:guid}")]
    [Authorise]
    public async Task<IActionResult> UpdatePlaceInfo(Guid placeId)
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get place information by field name
    /// </summary>
    [HttpGet("{placeId:guid}/field/{fieldName}")]
    [Authorise]
    public async Task<IActionResult> GetPlaceInfoByField(Guid placeId, string fieldName)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Update place information by field name
    /// </summary>
    [HttpPost("{placeId:guid}/field/{fieldName}")]
    [Authorise]
    public async Task<IActionResult> UpdatePlaceInfoByField(Guid placeId, string fieldName)
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// Update place's current activity
    /// </summary>
    [HttpPost("current")]
    [Authorise]
    public async Task<IActionResult> UpdatePlaceActivity()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Update place refresh key/api key
    /// </summary>
    [HttpPost("current/refreshkey")]
    [Authorise]
    public async Task<IActionResult> UpdatePlaceRefreshKey()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Retrieve a list of places with no associated domain
    /// </summary>
    [HttpGet("orphan")]
    [HttpGet("unhooked")]
    [HttpGet("/api/maint/places/unhooked")]
    [Authorise(UserRole.Admin)]
    public async Task<IActionResult> GetOrphanPlaces()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Delete places with no associated domain
    /// </summary>
    [HttpDelete("orphan")]
    [HttpDelete("unhooked")]
    [HttpDelete("/api/maint/places/unhooked")]
    [Authorise(UserRole.Admin)]
    public async Task<IActionResult> DeleteOrphanPlaces()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Retrieve a list of inactive places
    /// </summary>
    [HttpGet("inactive")]
    [HttpGet("/api/maint/places/inactive")]
    [Authorise(UserRole.Admin)]
    public async Task<IActionResult> GetInactivePlaces()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Delete inactive places
    /// </summary>
    [HttpDelete("inactive")]
    [HttpDelete("/api/maint/places/inactive")]
    [Authorise(UserRole.Admin)]
    public async Task<IActionResult> DeleteInactivePlaces()
    {
        //TODO
        throw new NotImplementedException();
    }
}