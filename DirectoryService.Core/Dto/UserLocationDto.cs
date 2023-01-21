using DirectoryService.Core.Entities;

namespace DirectoryService.Core.Dto;

public class UserLocationDto
{
    public Domain? Domain { get; set; }
    public string? Path { get; set; }
}