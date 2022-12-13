namespace DirectoryService.Shared;

public enum MaturityRating
{
    Unrated,
    Everyone,
    Teen,
    Mature,
    Adult
}

public static class MaturityRatingExtension
{
    public static string ToMaturityRatingString(this MaturityRating scope)
    {
        return scope switch
        {
            MaturityRating.Unrated => "unrated",
            MaturityRating.Everyone => "everyone",
            MaturityRating.Teen => "teen",
            MaturityRating.Mature => "mature",
            MaturityRating.Adult => "adult",
            _ => "unrated"
        };
    }

    public static MaturityRating ToMaturityRating(this string? scope)
    {
        if (scope == null)
            return MaturityRating.Unrated;

        return scope.ToLower() switch
        {
            "unrated" => MaturityRating.Unrated,
            "everyone" => MaturityRating.Everyone,
            "teen" => MaturityRating.Teen,
            "mature" => MaturityRating.Mature,
            "adult" => MaturityRating.Adult,
            _ => MaturityRating.Unrated
        };
    }
}