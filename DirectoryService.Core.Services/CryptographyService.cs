namespace DirectoryService.Core.Services;

// ReSharper disable once ClassNeverInstantiated.Global
public class CryptographyService
{
    /// <summary>
    /// In order to future proof for when new secure methods are implemented,
    /// if the version is below the current, re-hash password and store in db
    /// and set entity version to current
    /// </summary>
    private const int CurrentVersion = 1;

    public static GeneratedAuth GenerateAuth(string password)
    {
        var auth = new GeneratedAuth
        {
            Version = CurrentVersion,
            Hash = BCrypt.Net.BCrypt.HashPassword(password)
        };
        return auth;
    }

    public static bool AuthenticatePassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
    
    public class GeneratedAuth
    {
        public string? Hash { get; set; }
        public long Version { get; set; }
    }
    
}