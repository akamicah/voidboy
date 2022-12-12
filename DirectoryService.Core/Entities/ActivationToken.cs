namespace DirectoryService.Core.Entities;

public class ActivationToken : GuidIdentifiedEntity
{
    public Guid AccountId { get; set; }
    public DateTime Expires { get; set; }
}