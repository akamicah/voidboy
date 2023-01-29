namespace DirectoryService.Core.Entities;

public class UserPresence : BaseEntity
{
    public bool? Connected { get; set; }
    public Guid? DomainId { get; set; }
    public Guid? PlaceId { get; set; }
    public string? NetworkAddress { get; set; }
    public string? PublicKey { get; set; }
    public string? Path { get; set; }
    public DateTime LastHeartbeat { get; set; }
    public Guid? NodeId { get; set; }
    public string? Availability { get; set; }
}