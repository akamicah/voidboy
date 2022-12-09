namespace DirectoryService.Shared;

public enum Permission
{
    Invalid,
    None,
    All,
    Public,
    Domain,
    Owner,
    Friend,
    Connection,
    Admin,
    Sponsor,
    Manager,
    DomainAccess
}

public static class PermissionExtension
{
    public static string ToPermissionString(this Permission permission)
    {
        return permission switch
        {
            Permission.None => "none",
            Permission.All => "all",
            Permission.Public => "public",
            Permission.Domain => "domain",
            Permission.Owner => "owner",
            Permission.Friend => "friend",
            Permission.Connection => "connection",
            Permission.Admin => "admin",
            Permission.Sponsor => "sponsor",
            Permission.Manager => "manager",
            Permission.DomainAccess => "domainaccess",
            _ => "invalid"
        };
    }

    public static Permission ToPermission(this string? permission)
    {
        if (permission == null)
            return Permission.Invalid;
        
        return permission.ToLower() switch
        {
            "none" => Permission.None,
            "all" => Permission.All,
            "public" => Permission.Public,
            "domain" => Permission.Domain,
            "owner" => Permission.Owner,
            "friend" => Permission.Friend,
            "connection" => Permission.Connection,
            "admin" => Permission.Admin,
            "sponsor" => Permission.Sponsor,
            "manager" => Permission.Manager,
            "domainaccess" => Permission.DomainAccess,
            _ => Permission.Invalid
        };
    }
}