using AutoMapper;
using DirectoryService.Core.Dto;

namespace DirectoryService.Api.Controllers.V1.Models;

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