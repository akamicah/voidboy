namespace DirectoryService.Core.Dto;

public class TokenGrantRequestDto
{
    public string? GrantType { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? RefreshToken { get; set; }
    //public string? ClientId { get; set; }
    //public string? ClientSecret { get; set; }
    //public string? Code { get; set; }
    //public string? RedirectUrl { get; set; }
    //public string? Scope { get; set; }
}