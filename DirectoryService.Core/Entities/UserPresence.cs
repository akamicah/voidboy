namespace DirectoryService.Core.Entities;

public class UserPresence : BaseEntity
{
    public Guid? DomainId { get; set; }
    public string? PublicKey { get; set; }
    public string? Path { get; set; }
    public DateTime LastHeartbeat { get; set; }
}