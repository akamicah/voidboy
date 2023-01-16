using AutoMapper;
using DirectoryService.Core.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1.Models;

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
    
    public class ModelMapperProfile : Profile
    {
        public ModelMapperProfile()
        {
            CreateMap<UpdateDomainModel, UpdateDomainDto>();
        }
    }
}