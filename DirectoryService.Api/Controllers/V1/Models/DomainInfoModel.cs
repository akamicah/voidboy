using System.Text.Json.Serialization;
using DirectoryService.Core.Entities;
using DirectoryService.Shared;
using DirectoryService.Shared.Extensions;

namespace DirectoryService.Api.Controllers.V1.Models;

public class DomainInfoModel
{
    public Guid Id { get; set; }
    
    [JsonPropertyName("domainId")] 
    public Guid DomainId { get; set; }
    public string? Name { get; set; }
    public string? Visibility { get; set; }
    public int Capacity { get; set; }
    
    [JsonPropertyName("sponsorAccountId")]
    public Guid SponsorAccountId { get; set; }
    public string? Label { get; set; }
    public string? NetworkAddress { get; set; }
    public int NetworkPort { get; set; }
    public string? IceServerAddress { get; set; }
    public string? Version { get; set; }
    public string? ProtocolVersion { get; set; }
    public bool Active { get; set; }
    public string? TimeOfLastHeartbeat { get; set; }
    public long TimeOfLastHeartbeatS { get; set; }
    public long NumUsers { get; set; }
    public string? ApiKey { get; set; }

    public DomainInfoModel(Domain domain, bool includeApiKey = false)
    {
        Id = domain.Id;
        DomainId = domain.Id;
        Name = domain.Name;
        Visibility = domain.Visibility.ToDomainVisibilityString();
        Capacity = domain.Capacity;
        SponsorAccountId = domain.OwnerUserId;
        Label = domain.Name;
        NetworkAddress = domain.NetworkAddress;
        NetworkPort = domain.NetworkPort;
        IceServerAddress = domain.IceServerAddress;
        Version = domain.Version;
        ProtocolVersion = domain.ProtocolVersion;
        Active = domain.Active;
        TimeOfLastHeartbeat = domain.LastHeartbeat.ToUniversalIso8601();
        TimeOfLastHeartbeatS = domain.LastHeartbeat.ToMilliSecondsTimestamp();
        NumUsers = domain.AnonCount + domain.UserCount;
        if (includeApiKey)
            ApiKey = domain.SessionToken.ToString();
    }
}