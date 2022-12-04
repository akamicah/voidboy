namespace DirectoryService.Shared;

public enum TokenScope
{
    Invalid,
    Owner,
    Domain,
    Place
}

public static class TokenScopeExtension
{
    public static string ToScopeString(this TokenScope scope)
    {
        return scope switch
        {
            TokenScope.Owner => "owner",
            TokenScope.Domain => "domain",
            TokenScope.Place => "place",
            _ => "invalid"
        };
    }

    public static TokenScope ToTokenScope(this string? scope)
    {
        if (scope == null)
            return TokenScope.Invalid;
        
        return scope.ToLower() switch
        {
            "owner" => TokenScope.Owner,
            "domain" => TokenScope.Domain,
            "place" => TokenScope.Place,
            _ => TokenScope.Invalid
        };
    }
}