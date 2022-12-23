using System.Globalization;
using DirectoryService.Api.Attributes;
using DirectoryService.Api.Controllers.V1.Models;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Services;
using DirectoryService.Shared;
using DirectoryService.Shared.Extensions;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/domains")]
[ApiController]
public sealed class DomainsController : V1ApiController
{

    private readonly DomainService _domainService;

    public DomainsController(DomainService domainService)
    {
        _domainService = domainService;
    }
    
    /// <summary>
    /// Fetch a list of domains
    /// </summary>
    [HttpGet]
    [Authorise]
    public async Task<IActionResult> GetDomains()
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// Fetch information on provided domain
    /// </summary>
    [HttpGet("{domainId:guid}")]
    [Authorise]
    public async Task<IActionResult> GetDomain(Guid domainId)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Register a new domain
    /// </summary>
    [HttpPost]
    [Authorise]
    public async Task<IActionResult> RegisterDomain([FromBody] V1RegisterDomainModel registerDomainModel)
    {
        var registeredDomain = await _domainService.RegisterNewDomain(registerDomainModel.Domain);

        var managers = await _domainService.GetDomainManagers(registeredDomain.RegisteredDomain!.Id);

        var response = new
        {
            Domain = new
            {
                Id = registeredDomain.RegisteredDomain.Id,
                DomainId = registeredDomain.RegisteredDomain.Id,
                Name = registeredDomain.RegisteredDomain.Name,
                Visiblity = registeredDomain.RegisteredDomain.Visibility.ToDomainVisibilityString(),
                Capacity = registeredDomain.RegisteredDomain.Capacity,
                SponsorAccountId = registeredDomain.RegisteredDomain.OwnerUserId,
                Label = registeredDomain.RegisteredDomain.Name,
                NetworkAddress = registeredDomain.RegisteredDomain.NetworkAddress,
                NetworkPort = registeredDomain.RegisteredDomain.NetworkPort,
                IceServerAddress = registeredDomain.RegisteredDomain.IceServerAddress,
                Version = registeredDomain.RegisteredDomain.Version,
                ProtocolVersion = registeredDomain.RegisteredDomain.ProtocolVersion,
                Active = registeredDomain.RegisteredDomain.Active,
                TimeOfLastHeartbeat = registeredDomain.RegisteredDomain.LastHeartbeat.ToString(CultureInfo.InvariantCulture),
                TimeOfLastHeartbeatS = registeredDomain.RegisteredDomain.LastHeartbeat.ToMilliSecondsTimestamp(),
                NumUsers = registeredDomain.RegisteredDomain.AnonCount + registeredDomain.RegisteredDomain.UserCount,
                ApiKey = registeredDomain.RegisteredDomain.SessionToken
            },
            Place = new
            {
                Id = registeredDomain.RegisteredPlace!.Id,
                PlaceId = registeredDomain.RegisteredPlace.Id,
                Name = registeredDomain.RegisteredPlace.Name,
                DisplayName = registeredDomain.RegisteredPlace.Name,
                Visibility = registeredDomain.RegisteredPlace.Visibility.ToDomainVisibilityString(),
                Address = registeredDomain.RegisteredPlace.Path,
                Path = registeredDomain.RegisteredPlace.Path,
                Description = registeredDomain.RegisteredPlace.Description,
                Maturity = registeredDomain.RegisteredPlace.Maturity.ToMaturityRatingString(),
                Tags = registeredDomain.RegisteredPlace.Tags,
                Managers = managers.Select(m => m.Username).ToList(),
                Thumbnail = registeredDomain.RegisteredPlace.ThumbnailUrl,
                Images = registeredDomain.RegisteredPlace.ImageUrls,
                CurrentAttendance = registeredDomain.RegisteredPlace.Attendance,
                CurrentImages = registeredDomain.RegisteredPlace.ImageUrls,
                CurrentInfo = "{}",
                CurrentLastUpdateTime = registeredDomain.RegisteredPlace.UpdatedAt?.ToString(CultureInfo.InvariantCulture),
                CurrentLastUpdateTimeS = registeredDomain.RegisteredPlace.UpdatedAt?.ToMilliSecondsTimestamp(),
                LastActivityUpdate = registeredDomain.RegisteredPlace.LastActivity.ToString(CultureInfo.InvariantCulture),
                LastActivityUpdateS = registeredDomain.RegisteredPlace.LastActivity.ToMilliSecondsTimestamp()
            }
        };
        
        return Success(response);
    }

    /// <summary>
    /// Update domain information
    /// </summary>
    [HttpPut("{domainId:guid}")]
    [Authorise]
    public async Task<IActionResult> UpdateDomain(Guid domainId, [FromBody] UpdateDomainDto domainUpdate)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Delete domain
    /// </summary>
    [HttpDelete("{domainId:guid}")]
    [Authorise]
    public async Task<IActionResult> DeleteDomain(Guid domainId)
    {
        await _domainService.DeleteDomain(domainId);
        return Success();
    }

    /// <summary>
    /// Fetch specific information about domain
    /// </summary>
    [HttpGet("{domainId:guid}/field/{fieldName}")]
    [Authorise]
    public async Task<IActionResult> GetDomainProperty(Guid domainId, string fieldName)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Update specific information about domain
    /// </summary>
    [HttpPost("{domainId:guid}/field/{fieldName}")]
    [Authorise]
    public async Task<IActionResult> SetDomainProperty(Guid domainId, string fieldName, [FromBody] UpdateFieldDto fieldUpdate)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Set ICE server address
    /// </summary>
    [HttpPut("{domainId:guid}/ice_server_address")]
    [Authorise]
    public async Task<IActionResult> SetDomainIceServer(Guid domainId, [FromBody] V1UpdateIceServerModel updateIceServerModel)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Get the public key of the domain
    /// </summary>
    [HttpGet("{domainId:guid}/public_key")]
    [Authorise]
    public async Task<IActionResult> GetPublicKey(Guid domainId)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Update the public key of the domain
    /// </summary>
    [HttpPut("{domainId:guid}/public_key")]
    [Authorise]
    public async Task<IActionResult> SetPublicKey(Guid domainId)
    {
        //TODO
        throw new NotImplementedException();
    }

 
    
    /// <summary>
    /// Register a temporary domain
    /// </summary>
    [HttpPost("temporary")]
    [Authorise]
    public async Task<IActionResult> AddTemporaryDomain()
    {
        //TODO
        throw new NotImplementedException();
    }
    
}