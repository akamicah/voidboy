namespace DirectoryService.Core.Dto;

public class DomainReferenceDto
{
    public Guid? Id { get; set; }
    public string? NetworkAddress { get; set; }
    public int? NetworkPort { get; set; }
    public string? IceServerAddress { get; set; }
    public string? Name { get; set; }
}