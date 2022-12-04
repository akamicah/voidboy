using System.Text.Json.Serialization;

namespace DirectoryService.Core.Dto;

public class GrantedTokenDto
{
    [JsonPropertyName("access_token")] public string? AccessToken { get; set; }
    [JsonPropertyName("token_type")] public string? TokenType { get; set; }
    [JsonPropertyName("expires_in")] public long ExpiresIn { get; set; }
    [JsonPropertyName("refresh_token")] public string? RefreshToken { get; set; }
    public string? Scope { get; set; }
    [JsonPropertyName("created_at")] public long CreatedAt { get; set; }
    [JsonPropertyName("account_name")] public string? AccountName { get; set; }
    [JsonPropertyName("account_roles")] public List<string>? AccountRoles { get; set; }
    [JsonPropertyName("account_id")] public string? AccountId { get; set; }
}