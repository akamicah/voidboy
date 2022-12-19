using DirectoryService.Shared;

namespace DirectoryService.Core.Entities;

public class Place : BaseEntity
{
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public DomainVisibility Visibility { get; set; }
    public MaturityRating Maturity { get; set; }
    public string? Path { get; set; }
    public string? ThumbnailUrl { get; set; }
    public List<string>? ImageUrls { get; set; }
    public string? CurrentInfo { get; set; }
    public int CurrentAttendance { get; set; }
    public DateTime CurrentLastUpdateTime { get; set; }
    public Guid CurrentApiKeyTokenId { get; set; }
    public string? CreatorIp { get; set; }
    public DateTime LastActivity { get; set; }
}