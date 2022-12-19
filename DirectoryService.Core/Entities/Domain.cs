using DirectoryService.Shared;

namespace DirectoryService.Core.Entities;

public class Domain : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ContactInfo { get; set; }
    public List<string>? HostNames { get; set; }
    public Uri? ThumbnailUrl { get; set; }
    public List<string>? Images { get; set; }
    public MaturityRating Maturity { get; set; }
    public DomainVisibility Visibility { get; set; }
    public string? PublicKey { get; set; }
    public Guid ApiKey { get; set; }
    public Guid SponsorUserId { get; set; }
    public Uri? IceServerAddress { get; set; }
    public string? Version { get; set; }
    public string? Protocol { get; set; }
    public string? NetworkAddress { get; set; }
    public int NetworkPort { get; set; }
    public NetworkingMode NetworkingMode { get; set; }
    public bool Restricted { get; set; }
    public int NumUsers { get; set; }
    public int AnonUsers { get; set; }
    public int Capacity { get; set; }
    public DomainRestriction Restriction { get; set; }
    public List<string>? Tags { get; set; }
    public string? RegisterIp { get; set; }
    public DateTime LastHeartbeat { get; set; }
    public string? LastSenderKey { get; set; }
}