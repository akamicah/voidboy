using DirectoryService.Core.Entities;
using DirectoryService.Shared;
using DirectoryService.Shared.Extensions;

namespace DirectoryService.Api.Controllers.V1.Models;

public class V1PlaceInfoSmallModel
{
    public Guid Id { get; set; }
    public Guid PlaceId { get; set; }
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Visibility { get; set; }
    public string? Address { get; set; }
    public string? Path { get; set; }
    public string? Description { get; set; }
    public string? Maturity { get; set; }
    public List<string>? Tags { get; set; }
    public List<string>? Managers { get; set; }
    public string? Thumbnail { get; set; }
    public List<string>? Images { get; set; }
    public long CurrentAttendance { get; set; }
    public List<string>? CurrentImages { get; set; }
    public string? CurrentInfo { get; set; }
    public string? CurrentLastUpdateTime { get; set; }
    public long? CurrentLastUpdateTimeS { get; set; }
    public string? LastActivityUpdate { get; set; }
    public long? LastActivityUpdateS { get; set; }

    public V1PlaceInfoSmallModel(Place place, IEnumerable<User> managers)
    {
        Id = place.Id;
        PlaceId = place.Id;
        Name = place.Name;
        DisplayName = place.Name;
        Visibility = place.Visibility.ToDomainVisibilityString();
        Address = place.Path;
        Path = place.Path;
        Description = place.Description;
        Maturity = place.Maturity.ToMaturityRatingString();
        Tags = place.Tags;
        Managers = managers.Select(m => m.Username!).ToList();
        Thumbnail = place.ThumbnailUrl;
        Images = place.ImageUrls;
        CurrentAttendance = place.Attendance;
        CurrentImages = place.ImageUrls;
        CurrentInfo = "{}";
        CurrentLastUpdateTime = place.UpdatedAt.ToUniversalIso8601();
        CurrentLastUpdateTimeS = place.UpdatedAt.ToMilliSecondsTimestamp();
        LastActivityUpdate = place.LastActivity.ToUniversalIso8601();
        LastActivityUpdateS = place.LastActivity.ToMilliSecondsTimestamp();
    }
}