using DirectoryService.Core.Entities;

namespace DirectoryService.Core.Dto;

public class RegisteredDomainDto
{
    public Domain? RegisteredDomain { get; set; }
    public Place? RegisteredPlace { get; set; }
}