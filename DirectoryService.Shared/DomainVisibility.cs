namespace DirectoryService.Shared;

//TODO: Figure out what options there are supposed to be
public enum DomainVisibility
{
    Invalid,
    Open,
}

public static class DomainVisibilityExtension
{
    public static string ToDomainVisibilityString(this DomainVisibility visibility)
    {
        return visibility switch
        {
            DomainVisibility.Open => "open",
            _ => "invalid"
        };
    }

    public static DomainVisibility ToDomainVisibility(this string? visibility)
    {
        if (visibility == null)
            return DomainVisibility.Invalid;

        return visibility.ToLower() switch
        {
            "open" => DomainVisibility.Open,
            _ => DomainVisibility.Invalid
        };
    }
}