using AutoMapper;
using DirectoryService.Core.Dto;

namespace DirectoryService.Api.Controllers.V1.Models;


public class UserHeartbeatRootModel
{

    public UserHeartbeatModel? Location { get; set; }

    public class UserHeartbeatModel
    {
        public bool? Connected { get; set; }
        public string? Path { get; set; }
        public Guid? DomainId { get; set; }
        public Guid? PlaceId { get; set; }
        public string? NetworkAddress { get; set; }
        public Guid? NodeId { get; set; }
        public string? Availability { get; set; }
    }
    
    public class ModelMapperProfile : Profile
    {
        public ModelMapperProfile()
        {
            CreateMap<UserHeartbeatModel, UserHeartbeatDto>();
        }
    }
}