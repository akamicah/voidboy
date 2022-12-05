using System.Net;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Tests.IntegrationTests;

[TestFixture]
public class UserServiceTests : TestBase
{
    [OneTimeSetUp]
    public void Setup()
    {
        TestSetup();
    }

    [Test]
    public async Task CanRegisterUserTest()
    {
        var userService = _factory!.Services.GetRequiredService<UserService>();

        var user = await userService.RegisterUser(new RegisterUserDto()
        {
            Email = "test123@test.com",
            Username = "test123",
            Password = "test123!",
            OriginIp = IPAddress.Any
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(user.AccountId, Is.Not.Empty);
            Assert.That(user.Username!, Is.EqualTo("test123"));
        });
    }
}