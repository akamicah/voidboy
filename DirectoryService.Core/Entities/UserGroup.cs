using DirectoryService.Shared;

namespace DirectoryService.Core.Entities;

public class UserGroup : BaseEntity
{
    public Guid OwnerUserId { get; set; }
    public bool Internal { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public MaturityRating Rating { get; set; }
}