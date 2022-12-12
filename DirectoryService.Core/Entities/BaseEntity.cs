namespace DirectoryService.Core.Entities;

public class BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class GuidIdentifiedEntity : BaseEntity
{
    public Guid Id { get; set; }
}