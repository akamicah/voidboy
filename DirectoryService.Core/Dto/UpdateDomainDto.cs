namespace DirectoryService.Core.Dto;

public class UpdateDomainDto
{
    public Guid DomainId { get; set; }
    public string? Name { get; set; }
    public string? Version { get; set; }
    public string? Protocol { get; set; }
    public string? NetworkAddress { get; set; }
    public bool? Restricted { get; set; }
    public int? Capacity { get; set; }
    public string? Description { get; set; }
    public string? Maturity { get; set; }
    public string? Restriction { get; set; }
    public List<string>? Managers { get; set; }
    public List<string>? Tags { get; set; }
    public DomainHeartbeatDto? Heartbeat { get; set; }
}