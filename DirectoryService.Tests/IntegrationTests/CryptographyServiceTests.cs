using DirectoryService.Core.Services;

namespace DirectoryService.Tests.IntegrationTests;

public class CryptographyServiceTests : TestBase
{
    [OneTimeSetUp]
    public void Setup()
    {
        TestSetup();
    }

    [Test]
    public Task CryptographicHashingTests()
    {
        const string password = "Password1234!";
        const string badPassword = "Password123!";

        var hash1 = CryptographyService.GenerateAuth(password);
        var hash2 = CryptographyService.GenerateAuth(password);
        var hash3 = CryptographyService.GenerateAuth(password);
        
        Assert.Multiple(() =>
        {
            Assert.That(hash1.Hash, Is.Not.Null);
            Assert.That(hash2.Hash, Is.Not.Null);
            Assert.That(hash3.Hash, Is.Not.Null);
        });
        
        // Ensure unique hashes
        Assert.Multiple(() =>
        {
            Assert.That(hash1.Hash, Is.Not.EqualTo(hash2.Hash));
            Assert.That(hash1.Hash, Is.Not.EqualTo(hash3.Hash));
            Assert.That(hash2.Hash, Is.Not.EqualTo(hash3.Hash));
        });
        
        // Ensure Authentication
        Assert.Multiple(() =>
        {
            Assert.That(CryptographyService.AuthenticatePassword(password, hash1.Hash!), Is.True);
            Assert.That(CryptographyService.AuthenticatePassword(password, hash2.Hash!), Is.True);
            Assert.That(CryptographyService.AuthenticatePassword(password, hash3.Hash!), Is.True);
        });
        
        // Ensure Authentication Fail
        Assert.Multiple(() =>
        {
            Assert.That(CryptographyService.AuthenticatePassword(badPassword, hash1.Hash!), Is.False);
            Assert.That(CryptographyService.AuthenticatePassword(badPassword, hash2.Hash!), Is.False);
            Assert.That(CryptographyService.AuthenticatePassword(badPassword, hash3.Hash!), Is.False);
        });
        
        return Task.CompletedTask;
    }
}