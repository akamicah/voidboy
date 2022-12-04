namespace DirectoryService.Core.Entities;

public class BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Deleted { get; set; }
}

public class IdentifiedEntity : BaseEntity
{
    public Guid Id { get; set; }
}