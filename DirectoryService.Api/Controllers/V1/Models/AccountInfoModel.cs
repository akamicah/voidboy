using System.Text.Json.Serialization;
using DirectoryService.Core.Dto;
using DirectoryService.Shared;
using DirectoryService.Shared.Extensions;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DirectoryService.Api.Controllers.V1.Models;

public class AccountInfoModel
{
    public Guid Id { get; set; }

    [JsonPropertyName("accountId")] public Guid AccountId { get; set; }

    public string? Username { get; set; }
    public string? Email { get; set; }
    public bool Administrator { get; set; }
    public bool Enabled { get; set; }
    public List<string>? Roles { get; set; }
    public List<string>? Availability { get; set; }
    public string? PublicKey { get; set; }
    public UserImagesDto? Images { get; set; }
    public string? ProfileDetail { get; set; }
    public LocationModel? Location { get; set; }
    public List<string>? Friends { get; set; }
    public List<string>? Connections { get; set; }
    public string? WhenAccountCreated { get; set; }
    public long WhenAccountCreatedS { get; set; }
    public string? TimeOfLastHeartbeat { get; set; }
    public long? TimeOfLastHeartbeatS { get; set; }

    public AccountInfoModel(UserInfoDto userInfo)
    {
        Id = userInfo.Id;
        AccountId = userInfo.Id;
        Username = userInfo.Username;
        Email = userInfo.Email;
        Administrator = userInfo.Role == UserRole.Admin;
        Enabled = userInfo.Enabled;
        if (userInfo.Role == UserRole.Admin)
        {
            Roles = new List<string>()
            {
                UserRole.User.ToRoleString(),
                UserRole.Admin.ToRoleString()
            };
        }
        else
        {
            Roles = new List<string>()
            {
                UserRole.User.ToRoleString()
            };
        }

        Availability = userInfo.Availability;
        PublicKey = userInfo.PublicKey;
        Images = userInfo.Images;
        ProfileDetail = userInfo.ProfileDetail;
        Location = new LocationModel(userInfo.Location!);
        Friends = new List<string>();
        Connections = new List<string>();
        WhenAccountCreated = userInfo.CreationDate.ToUniversalIso8601();
        WhenAccountCreatedS = userInfo.CreationDate.ToMilliSecondsTimestamp();
        TimeOfLastHeartbeat = userInfo.LastHeartbeat?.ToUniversalIso8601();
        TimeOfLastHeartbeatS = userInfo.LastHeartbeat?.ToMilliSecondsTimestamp();
    }

    public class LocationModel
    {
        public DomainInfoModel? Root { get; set; }
        public string? Path { get; set; }

        public LocationModel(UserLocationDto location)
        {
            Root = location.Domain != null ? new DomainInfoModel(location.Domain!, false) : null;
            Path = location.Domain != null ? location.Path : null;
        }
    }
}