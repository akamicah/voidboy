namespace DirectoryService.Core.Entities;

public class ActivationToken : IdentifiedEntity
{
    public Guid AccountId { get; set; }
    public DateTime Expires { get; set; }
}