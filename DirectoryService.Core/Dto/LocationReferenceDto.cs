namespace DirectoryService.Core.Dto;

public class RootLocationReferenceDto
{
    public DomainReferenceDto? Domain { get; set; }
}

public class LocationReferenceDto
{
    public Guid? NodeId { get; set; }
    public RootLocationReferenceDto? Root { get; set; }
    public string? Path { get; set; }
    public bool? Online { get; set; }
}