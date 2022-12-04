using System.Text.Json.Serialization;

namespace DirectoryService.Core.Dto;

// ReSharper disable UnusedAutoPropertyAccessor.Global

public class UserRegisteredDto
{
    [JsonPropertyName("accountId")]
    public string? AccountId { get; set; }
    public string? Username { get; set; }
    
    [JsonPropertyName("accountIsActive")]
    public bool AccountIsActive { get; set; }
    
    [JsonPropertyName("accountWaitingVerification")]
    public bool AccountAwaitingVerification { get; set; }
}