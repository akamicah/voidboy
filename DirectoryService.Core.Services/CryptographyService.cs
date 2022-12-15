using System.Security.Cryptography;
using DirectoryService.Shared.Attributes;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Core.Services;

// ReSharper disable once ClassNeverInstantiated.Global

[ScopedDependency]
public class CryptographyService
{
    private readonly ILogger<CryptographyService> _logger;

    public CryptographyService(ILogger<CryptographyService> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// In order to future proof for when new secure methods are implemented,
    /// if the version is below the current, re-hash password and store in db
    /// and set entity version to current
    /// </summary>
    private const int CurrentVersion = 1;

    /// <summary>
    /// Generate an Auth object containing a hash and the auth version
    /// </summary>
    public static GeneratedAuth GenerateAuth(string password)
    {
        var auth = new GeneratedAuth
        {
            Version = CurrentVersion,
            Hash = BCrypt.Net.BCrypt.HashPassword(password)
        };
        return auth;
    }

    public class GeneratedAuth
    {
        public string? Hash { get; set; }
        public long Version { get; set; }
    }
    
    /// <summary>
    /// Authenticate a password against a hash. Version is unused for now
    /// </summary>
    public static bool AuthenticatePassword(string password, string hash, int version = 1)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    /// <summary>
    /// Convert a PKCS1 in DER binary format into SPKI in PEM format
    /// </summary>
    public async Task<string> ConvertPkcs1Key(byte[] pkcs1Key)
    {
        try
        {
            var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(pkcs1Key, out var bytesRead);
            var pem = "";
            if (bytesRead == 0)
            {
                _logger.LogError(
                    "An error occured converting RSA public key from PKCS1 (DER) to SPKI (PEM). Bytes read: 0");
            }
            else
            {
                var spki = rsa.ExportSubjectPublicKeyInfo();
                pem = Convert.ToBase64String(spki);
            }
            rsa.Clear();
            return pem;
        }
        catch (Exception e)
        {
            _logger.LogError("An error occured converting RSA public key from PKCS1 (DER) to SPKI (PEM). {exception}", e);
        }

        return "";
    }
}