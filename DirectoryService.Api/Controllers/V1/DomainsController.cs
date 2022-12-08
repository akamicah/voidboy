using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/domains")]
[ApiController]
public sealed class DomainsController : V1ApiController
{
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
        //TODO
        throw new NotImplementedException();
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
    public async Task<IActionResult> SetDomainIceServer(Guid domainId, [FromBody] UpdateIceServerModel iceServerUpdate)
    {
        //TODO
        throw new NotImplementedException();
    }

    public class UpdateIceServerModel
    {
        public IceServerModel Domain { get; set; }
    }
    
    //V2: Simplify request
    public class IceServerModel
    {
        public string? IceServerAddress { get; set; }
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
    /// Register a new domain
    /// </summary>
    [HttpPost]
    [Authorise]
    public async Task<IActionResult> AddDomain()
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