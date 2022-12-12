namespace DirectoryService.Core.Entities;

public class UserConnection : BaseEntity
{
    public Guid UserAId { get; set; }
    public Guid UserBId { get; set; }
}