using System.Text.Json.Serialization;
using DirectoryService.Core.Entities;
using DirectoryService.Shared;
using DirectoryService.Shared.Extensions;

namespace DirectoryService.Api.Controllers.V1.Models;

public class DomainInfoModelV1
{
    public Guid Id { get; set; }
    
    [JsonPropertyName("domainId")] 
    public Guid DomainId { get; set; }
    public string? Name { get; set; }
    public string? Visibility { get; set; }
    public string? WorldName { get; set; }
    public string? Label { get; set; }
    public string? PublicKey { get; set; }
    public List<PlaceInfoSmallModel>? OwnerPlaces { get; set; }
    
    [JsonPropertyName("sponsorAccountId")]
    public Guid SponsorAccountId { get; set; }
    public string? IceServerAddress { get; set; }
    public string? Version { get; set; }
    public string? ProtocolVersion { get; set; }
    public string? NetworkAddress { get; set; }
    public int NetworkPort { get; set; }
    public string? AutomaticNetworking { get; set; }
    public bool Restricted { get; set; }
    public int NumUsers { get; set; }
    public int AnonUsers { get; set; }
    public int TotalUsers { get; set; }
    public int Capacity { get; set; }
    public string? Description { get; set; }
    public string? Maturity { get; set; }
    public string? Restriction { get; set; }
    public List<string>? Managers { get; set; }
    public List<string>? Tags { get; set; }
    public MetaModel? Meta { get; set; }
    public UsersModel? Users { get; set; }
    public string? TimeOfLastHeartbeat { get; set; }
    public long TimeOfLastHeartbeatS { get; set; }
    public string? LastSenderKey { get; set; }
    public string? AddrOfFirstContact { get; set; }
    public string? WhenDomainEntryCreated { get; set; }
    public long WhenDomainEntryCreatedS { get; set; }

    public DomainInfoModelV1(Domain domain, List<User> managers, List<Place> domainPlaces)
    {
        Id = domain.Id;
        DomainId = domain.Id;
        Name = domain.Name;
        WorldName = domain.Name;
        Visibility = domain.Visibility.ToDomainVisibilityString();
        Capacity = domain.Capacity;
        SponsorAccountId = domain.OwnerUserId;
        Label = domain.Name;
        NetworkAddress = domain.NetworkAddress;
        NetworkPort = domain.NetworkPort;
        AutomaticNetworking = domain.NetworkingMode.ToNetworkingModeString();
        Restricted = domain.Restricted;
        NumUsers = domain.UserCount;
        AnonUsers = domain.AnonCount;
        TotalUsers = domain.UserCount + domain.AnonCount;
        Description = domain.Description;
        Maturity = domain.Maturity.ToMaturityRatingString();
        Restriction = domain.Restriction.ToDomainRestrictionString();
        Managers = managers.Select(m => m.Username!).ToList();
        Tags = domain.Tags;
        Meta = new MetaModel()
        {
            Capacity = domain.Capacity,
            ContactInfo = domain.ContactInfo,
            Description = domain.Description,
            Images = domain.ImageUrls,
            Managers = managers.Select(m => m.Username!).ToList(),
            Restriction = domain.Restriction.ToDomainRestrictionString(),
            Tags = domain.Tags,
            Thumbnail = domain.ThumbnailUrl,
            WorldName = domain.Name
        };
        Users = new UsersModel()
        {
            NumUsers = domain.UserCount,
            NumAnonUsers = domain.AnonCount,

            //TODO: Is this even used?
            //UserHostnames = new List<string>()
        };
        LastSenderKey = null;
        AddrOfFirstContact = domain.CreatorIp;
        IceServerAddress = domain.IceServerAddress;
        Version = domain.Version;
        ProtocolVersion = domain.ProtocolVersion;
        TimeOfLastHeartbeat = domain.LastHeartbeat.ToUniversalIso8601();
        TimeOfLastHeartbeatS = domain.LastHeartbeat.ToMilliSecondsTimestamp();
        WhenDomainEntryCreated = domain.CreatedAt.ToUniversalIso8601();
        WhenDomainEntryCreatedS = domain.CreatedAt.ToMilliSecondsTimestamp();
    }
    
    
    public class MetaModel
    {
        public int Capacity { get; set; }
        public string? ContactInfo { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; }
        public List<string>? Managers { get; set; }
        public string? Restriction { get; set; }
        public List<string>? Tags { get; set; }
        public string? Thumbnail { get; set; }
        public string? WorldName { get; set; }
    }

    public class UsersModel
    {
        public int NumAnonUsers { get; set; }
        public int NumUsers { get; set; }
        //public List<string>? UserHostnames { get; set; }
    }
}

