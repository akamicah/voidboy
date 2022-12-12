using DirectoryService.Shared;

namespace DirectoryService.Core.Entities;

public class SessionToken : GuidIdentifiedEntity
{
    public Guid RefreshToken { get; set; }
    public Guid AccountId { get; set; }
    public TokenScope Scope { get; set; }
    public DateTime Expires { get; set; }
}