namespace DirectoryService.Core.Dto;

public class UserHeartbeatDto
{
    public Guid UserId { get; set; }
    public bool Connected { get; set; }
    public string? Path { get; set; }
    public Guid? DomainId { get; set; }
    public Guid? PlaceId { get; set; }
    public string? NetworkAddress { get; set; }
    public Guid? NodeId { get; set; }
    public string? Availability { get; set; }
    
}