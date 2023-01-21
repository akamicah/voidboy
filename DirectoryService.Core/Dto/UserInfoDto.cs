using DirectoryService.Core.Entities;
using DirectoryService.Shared;

namespace DirectoryService.Core.Dto;

public class UserInfoDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public bool Administrator { get; set; }
    public bool Enabled { get; set; }
    public UserRole Role { get; set; }
    public List<string>? Availability { get; set; }
    public string? PublicKey { get; set; }
    public UserImagesDto? Images { get; set; }
    public string? ProfileDetail { get; set; }
    public UserLocationDto? Location { get; set; }
    public List<User>? Friends { get; set; }
    public List<User>? Connections { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? LastHeartbeat { get; set; }
}