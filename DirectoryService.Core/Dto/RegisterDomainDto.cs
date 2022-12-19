using System.Net;

namespace DirectoryService.Core.Dto;

public class RegisterDomainDto
{
    public string? Name { get; set; }
    public string? NetworkAddress { get; set; }
    public int NetworkPort { get; set; }
    public IPAddress? OriginIp { get; set; }
    
}