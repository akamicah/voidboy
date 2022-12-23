using System.Text.Json.Serialization;
using DirectoryService.Core.Entities;
using DirectoryService.Shared;

namespace DirectoryService.Api.Controllers.V1.Models;
   
/// <summary>
/// Model for V1 of the API
/// </summary>
public class V1UserTokenListModel
{
    public V1UserTokenListModel()
    {
        Tokens = new List<TokenModel>();
    }

    public V1UserTokenListModel(IEnumerable<SessionToken> sessionTokens)
    {
        Tokens = new List<TokenModel>();
        foreach (var sessionToken in sessionTokens)
        {
            Tokens.Add(new TokenModel()
            {
                CreatedOn = sessionToken.CreatedAt,
                ExpiresOn = sessionToken.Expires,
                Id = sessionToken.Id,
                RefreshToken = sessionToken.RefreshToken,
                Token = sessionToken.Id,
                TokenId = sessionToken.Id,
                Scope = new []
                {
                    sessionToken.Scope.ToScopeString()
                }
            });
        }
    }
        
    [JsonPropertyName("tokens")]
    public List<TokenModel> Tokens { get; set; }

    public class TokenModel
    {
        [JsonPropertyName("id")] 
        public Guid Id { get; set; }
        
        [JsonPropertyName("tokenid")]
        public Guid TokenId { get; set; }
        
        [JsonPropertyName("token")]
        public Guid Token { get; set; }
        
        [JsonPropertyName("refresh_token")]
        public Guid RefreshToken { get; set; }
        
        [JsonPropertyName("token_creation_time")]
        public DateTime CreatedOn { get; set; }
        
        [JsonPropertyName("token_expiration_time")]
        public DateTime ExpiresOn { get; set; }
        
        [JsonPropertyName("scope")]
        public IEnumerable<string>? Scope { get; set; }
    }
}