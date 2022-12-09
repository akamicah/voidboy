namespace DirectoryService.Core.Dto;

public class UpdateFieldDto
{
    public List<string>? Set { get; set; }
    public List<string>? Add { get; set; }
    public List<string>? Remove { get; set; }
}