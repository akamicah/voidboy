using DirectoryService.Core.Dto;

namespace DirectoryService.Api.Controllers.V1.Models;

public class V1RegisterPlaceModel
{
    public PlaceModel Place { get; set; }

    public class PlaceModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public Guid DomainId { get; set; }

        public RegisterPlaceDto ToDto()
        {
            return new RegisterPlaceDto()
            {
                Name = Name,
                Description = Description,
                Path = Address,
                DomainId = DomainId
            };
        }
    }
}