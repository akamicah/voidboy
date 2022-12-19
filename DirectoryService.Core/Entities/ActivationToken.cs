namespace DirectoryService.Core.Entities;

public class ActivationToken : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime Expires { get; set; }
}