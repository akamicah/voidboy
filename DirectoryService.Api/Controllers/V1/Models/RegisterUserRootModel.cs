using AutoMapper;
using DirectoryService.Core.Dto;

namespace DirectoryService.Api.Controllers.V1.Models;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

public class RegisterUserRootModel
{
    public RegisterUserModel? User { get; set; }

    public class RegisterUserModel {
        public string? Username { get; set; }
        public string? Password { get; set;}
        public string? Email { get; set;}
    }
    
    public class ModelMapperProfile : Profile
    {
        public ModelMapperProfile()
        {
            CreateMap<RegisterUserModel, RegisterUserDto>();
        }
    }
}