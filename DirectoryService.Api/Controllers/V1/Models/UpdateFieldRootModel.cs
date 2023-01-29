using System.Text.Json;

namespace DirectoryService.Api.Controllers.V1.Models;

public class UpdateFieldRootModel
{
    public JsonElement? Set { get; set; }
}