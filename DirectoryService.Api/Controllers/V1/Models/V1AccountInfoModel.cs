using System.Text.Json.Serialization;
using DirectoryService.Core.Entities;
using DirectoryService.Shared;

namespace DirectoryService.Api.Controllers.V1.Models;

public class V1AccountInfoModel
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
    public ImagesModel? Images { get; set; }
    public string? ProfileDetail { get; set; }
    public string? Location { get; set; }
    public List<string>? Friends { get; set; }
    public List<string>? Connections { get; set; }
    public string? WhenAccountCreated { get; set; }
    public long WhenAccountCreatedS { get; set; }
    public string? TimeOfLastHeartbeat { get; set; }
    public long TimeOfLastHeartbeatS { get; set; }

    public V1AccountInfoModel(User user)
    {
        Username = user.Username;
        Email = user.Email;
        Administrator = user.Role == UserRole.Admin;
        Enabled = user.Enabled;
        if (user.Role == UserRole.Admin)
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
    }

    public class ImagesModel
    {
        public string? Hero { get; set; }
        public string? Tiny { get; set; }
        public string? Thumbnail { get; set; }
    }

    public class LocationModel
    {
        public V1DomainInfoModel? Root { get; set; }
        public string? Path { get; set; }
    }
}