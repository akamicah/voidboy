using AutoMapper;
using DirectoryService.Core.Dto;

namespace DirectoryService.Api.Controllers.V1.Models;

public class RegisterPlaceRootModel
{
    public RegisterPlaceModel? Place { get; set; }
        
    public class RegisterPlaceModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public Guid DomainId { get; set; }
    }
    
    public class ModelMapperProfile : Profile
    {
        public ModelMapperProfile()
        {
            CreateMap<RegisterPlaceRootModel.RegisterPlaceModel, RegisterPlaceDto>();
        }
    }
}



