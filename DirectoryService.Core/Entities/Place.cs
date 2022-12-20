using DirectoryService.Shared;

namespace DirectoryService.Core.Entities;

public class Place : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DomainVisibility Visibility { get; set; }
    public MaturityRating Maturity { get; set; }
    public List<string>? Tags { get; set; }
    public Guid DomainId { get; set; }
    public string? Path { get; set; }
    public string? ThumbnailUrl { get; set; }
    public List<string>? ImageUrls { get; set; }
    public int Attendance { get; set; }
    public Guid SessionToken { get; set; }
    public string? CreatorIp { get; set; }
    public DateTime LastActivity { get; set; }
}