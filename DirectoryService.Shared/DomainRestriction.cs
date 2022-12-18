namespace DirectoryService.Shared;

//TODO: Figure out what options there are supposed to be
public enum DomainRestriction
{
    Invalid,
    Open,
}

public static class DomainRestrictionExtension
{
    public static string ToDomainRestrictionString(this DomainRestriction restriction)
    {
        return restriction switch
        {
            DomainRestriction.Open => "open",
            _ => "invalid"
        };
    }

    public static DomainRestriction ToDomainRestriction(this string? restriction)
    {
        if (restriction == null)
            return DomainRestriction.Invalid;

        return restriction.ToLower() switch
        {
            "open" => DomainRestriction.Open,
            _ => DomainRestriction.Invalid
        };
    }
}