namespace DirectoryService.Core.Entities;

public class ActivationToken : BaseEntity
{
    public Guid AccountId { get; set; }
    public DateTime Expires { get; set; }
}