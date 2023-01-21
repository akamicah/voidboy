using System.Net;
using AutoMapper;
using DirectoryService.Core.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1.Models;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

public class RegisterDomainRootModel
{
    public RegisterDomainInfoModel? Domain { get; set; }

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
    
    public class ModelMapperProfile : Profile
    {
        public ModelMapperProfile()
        {
            CreateMap<RegisterDomainInfoModel, RegisterDomainDto>();
        }
    }
}