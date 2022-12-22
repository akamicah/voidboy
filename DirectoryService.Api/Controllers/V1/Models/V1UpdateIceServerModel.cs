namespace DirectoryService.Api.Controllers.V1.Models;

public class V1UpdateIceServerModel
{
    public V1IceServerModel Domain { get; set; }
}
    
//V2: Simplify request
public class V1IceServerModel
{
    public string? IceServerAddress { get; set; }
}