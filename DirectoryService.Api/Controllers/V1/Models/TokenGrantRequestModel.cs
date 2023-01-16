using AutoMapper;
using DirectoryService.Core.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1.Models;

public class TokenGrantRequestModel
{
    [FromForm(Name = "grant_type")]
    public string? GrantType { get; set; }
        
    [FromForm(Name = "refresh_token")]
    public string? RefreshToken { get; set; }
        
    public string? Username { get; set; }
    public string? Password { get; set; }
    
    public class ModelMapperProfile : Profile
    {
        public ModelMapperProfile()
        {
            CreateMap<TokenGrantRequestModel, TokenGrantRequestDto>();
        }
    }
}