using DirectoryService.Core.Dto;

namespace DirectoryService.Api.Controllers.V1.Models;

public class V1RegisterPlaceModel
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

public class V1RegisterPlaceRootModel
{
    public V1RegisterPlaceModel Place { get; set; }
}