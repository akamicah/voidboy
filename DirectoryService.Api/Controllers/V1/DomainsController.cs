using System.Net;
using AutoMapper;
using DirectoryService.Api.Attributes;
using DirectoryService.Api.Controllers.V1.Models;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/domains")]
[ApiController]
public sealed class DomainsController : V1ApiController
{

    private readonly DomainService _domainService;
    private readonly IMapper _mapper;
    public DomainsController(DomainService domainService,
        IMapper mapper)
    {
        _domainService = domainService;
        _mapper = mapper;
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
    [AllowAnonymous]
    public async Task<IActionResult> GetDomain(Guid domainId)
    {
        var domain = await _domainService.FindById(domainId);
        if (domain is null) throw new DomainNotFoundApiException();

        return Success(new
        {
            Domain = new V1DomainInfoModel(domain)
        });
    }
    
    /// <summary>
    /// Register a new domain
    /// </summary>
    [HttpPost]
    [Authorise]
    public async Task<IActionResult> RegisterDomain([FromBodyOrDefault] RegisterDomainRootModel registerDomainModel)
    {
        var registeredDomain = await _domainService.RegisterNewDomain(_mapper.Map<RegisterDomainDto>(registerDomainModel.Domain));

        var managers = await _domainService.GetDomainManagers(registeredDomain.RegisteredDomain!.Id);

        var response = new
        {
            Domain = new V1DomainInfoModel(registeredDomain.RegisteredDomain, true),
            Place = new V1PlaceInfoSmallModel(registeredDomain.RegisteredPlace!, managers)
        };
        
        return Success(response);
    }

    public class RegisterDomainRootModel
    {
        public RegisterDomainInfoModel Domain { get; set; }

        public class RegisterDomainInfoModel
        {
            public string? Name { get; set; }
            
            [FromForm(Name = "network_address")]
            public string? NetworkAddress { get; set; }
            
            [FromForm(Name = "network_port")]
            public int NetworkPort { get; set; }
            
            [FromForm(Name = "origin_ip")]
            public IPAddress? OriginIp { get; set; }
        }
    }
    
    /// <summary>
    /// Update domain information
    /// </summary>
    [HttpPut("{domainId:guid}")]
    [Authorise]
    public async Task<IActionResult> UpdateDomain(Guid domainId, [FromBodyOrDefault] UpdateDomainRootModel domainUpdate)
    {
        var updateDto = _mapper.Map<UpdateDomainDto>(domainUpdate.Domain);
        updateDto.DomainId = domainId;
        await _domainService.UpdateDomain(updateDto);
        return Success();
    }

    public class UpdateDomainRootModel
    {
        public UpdateDomainModel Domain { get; set; }
        
        public class UpdateDomainModel
        {
            public string? Name { get; set; }
            public string? Version { get; set; }
            public string? Protocol { get; set; }
            
            [FromForm(Name = "network_address")]
            public string? NetworkAddress { get; set; }
            
            [FromForm(Name = "network_port")]
            public int? NetworkPort { get; set; }
            
            public bool? Restricted { get; set; }
            public int? Capacity { get; set; }
            public string? Description { get; set; }
            public string? Maturity { get; set; }
            public string? Restriction { get; set; }
            public List<string>? Managers { get; set; }
            public List<string>? Tags { get; set; }
            public DomainHeartbeatDto? Heartbeat { get; set; }
        }
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
    [AllowAnonymous]
    public async Task<IActionResult> GetDomainProperty(Guid domainId, string fieldName)
    {
        var value = await _domainService.GetDomainField(domainId, fieldName);
        return Success(value ?? new {});
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
        if (updateIceServerModel.Domain.IceServerAddress is null)
            return Failure();
        
        await _domainService.UpdateIceServerAddress(domainId, updateIceServerModel.Domain.IceServerAddress);
        return Success();
    }

    /// <summary>
    /// Get the public key of the domain
    /// </summary>
    [HttpGet("{domainId:guid}/public_key")]
    [Authorise]
    public async Task<IActionResult> GetPublicKey(Guid domainId)
    {
        var domain = await _domainService.FindById(domainId);
        if (domain is null) throw new DomainNotFoundApiException();
        return Success(new
        {
            PublicKey = domain.PublicKey != null ? CryptographyService.SimplifyPemKey(domain.PublicKey) : ""
        });
    }

    /// <summary>
    /// Update the public key of the domain
    /// </summary>
    [HttpPut("{domainId:guid}/public_key")]
    [Authorise]
    public async Task<IActionResult> SetPublicKey(Guid domainId)
    {
        var cert = HttpContext.Request.Form.Files.GetFile("public_key");
        if (cert is null)
            throw new BaseApiException("FileMissing", "The file 'public_key' was not included.", 400);

        await _domainService.UpdatePublicKey(domainId, cert.OpenReadStream());
        return Success();
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
    
    public class ModelMapperProfile : Profile
    {
        public ModelMapperProfile()
        {
            CreateMap<RegisterDomainRootModel.RegisterDomainInfoModel, RegisterDomainDto>();
            CreateMap<UpdateDomainRootModel.UpdateDomainModel, UpdateDomainDto>();
        }
    }
}