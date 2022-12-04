namespace DirectoryService.Shared;

public enum UserRole
{
    Invalid,
    User,
    Admin
}

public static class UserRoleExtension
{
    public static string ToRoleString(this UserRole role)
    {
        return role switch
        {
            UserRole.User => "user",
            UserRole.Admin => "admin",
            _ => "invalid"
        };
    }

    public static UserRole ToUserRole(this string role)
    {
        return role.ToLower() switch
        {
            "user" => UserRole.User,
            "admin" => UserRole.Admin,
            _ => UserRole.Invalid
        };
    }
}