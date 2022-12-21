namespace DirectoryService.Shared.Helpers;

public class StringHelper
{
    private static readonly Random Random = new Random();
    
    public static string GenerateRandomAlphanumericString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}