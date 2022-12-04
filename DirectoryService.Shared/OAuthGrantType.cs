namespace DirectoryService.Shared;

public enum OAuthGrantType
{
    Invalid,
    Password,
    AuthorisationCode,
    RefreshToken
}

public static class OAuthGrantTypeExtension
{
    public static string ToGrantTypeString(this OAuthGrantType grantType)
    {
        return grantType switch
        {
            OAuthGrantType.Password => "password",
            OAuthGrantType.AuthorisationCode => "authorization_code",
            OAuthGrantType.RefreshToken => "refresh_token",
            _ => "invalid"
        };
    }

    public static OAuthGrantType ToAuthGrantType(this string grantType)
    {
        return grantType.ToLower() switch
        {
            "password" => OAuthGrantType.Password,
            "authorization_code" => OAuthGrantType.AuthorisationCode,
            "refresh_token" => OAuthGrantType.RefreshToken,
            _ => OAuthGrantType.Invalid
        };
    }
}