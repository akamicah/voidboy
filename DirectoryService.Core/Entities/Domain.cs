using DirectoryService.Shared;

namespace DirectoryService.Core.Entities;

public class Domain : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ContactInfo { get; set; }
    public string? ThumbnailUrl { get; set; }
    public List<string>? ImageUrls { get; set; }
    public MaturityRating Maturity { get; set; }
    public DomainVisibility Visibility { get; set; }
    public string? PublicKey { get; set; }
    public Guid SessionToken { get; set; }
    public Guid OwnerUserId { get; set; }
    public string? IceServerAddress { get; set; }
    public string? Version { get; set; }
    public string? ProtocolVersion { get; set; }
    public string? NetworkAddress { get; set; }
    public int NetworkPort { get; set; }
    public NetworkingMode NetworkingMode { get; set; }
    public bool Restricted { get; set; }
    public int Capacity { get; set; }
    public DomainRestriction Restriction { get; set; }
    public List<string>? Tags { get; set; }
    public string? CreatorIp { get; set; }
    public DateTime LastHeartbeat { get; set; }
    public bool Active { get; set; }
    public int UserCount { get; set; }
    public int AnonCount { get; set; }
}