using System.Net;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.Services;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Shared;
using DirectoryService.Shared.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Tests.IntegrationTests;

[TestFixture]
public class UserServiceTests : TestBase
{
    [SetUp]
    public void Setup()
    {
        TestSetup();
    }

    [Test]
    public async Task CanRegisterUserTest()
    {
        var userService = _factory!.Services.GetRequiredService<UserService>();
        var emailService = _factory!.Services.GetRequiredService<IEmailService>();

        // We want an email to be sent
        ServiceConfigurationContainer.Config.Registration!.RequireEmailVerification = true;
        
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
        
        // Ensure activation email is queued for sending
        var emailQueue = await emailService.GetQueuedEmails();
        Assert.That(emailQueue, Is.Not.Empty);
    }
    
    [Test]
    public async Task CanLoginUserTest()
    {
        var userService = _factory!.Services.GetRequiredService<UserService>();

        var user = await userService.AuthenticateUser("testadmin", "password");
        
        Assert.Multiple(() =>
        {
            Assert.That(user.Id, Is.Not.Empty);
            Assert.That(user.Username!, Is.EqualTo("testadmin"));
            Assert.That(user.IdentityProvider!, Is.EqualTo(IdentityProvider.Local));
            Assert.That(user.Email!, Is.EqualTo("admin@test.com"));
        });
    }

    [Test]
    public async Task CanEditUserByFieldsTest()
    {
        var userService = _factory!.Services.GetRequiredService<UserService>();
        var sessionProvider = _factory!.Services.GetRequiredService<ISessionProvider>();

        var session = await sessionProvider.GetRequesterSession();
        
        await userService.UpdateUserByField(session!.UserId, "email", new[] { "test456@test.com" });
        await userService.UpdateUserByField(session!.UserId, "username", new[] { "test345" });

        var user = await userService.FindById(session.UserId);
        
        Assert.Multiple(() =>
        {
            Assert.That(user, Is.Not.Null);
            Assert.That(user!.Email!, Is.EqualTo("test456@test.com"));
            Assert.That(user!.Username!, Is.EqualTo("test345"));
        });
        
        // The following tests should fail:
        Assert.ThrowsAsync<UsernameTakenApiException>(
            async () => await userService.UpdateUserByField(session!.UserId, "username", new[] { "user" })
        );
        
        Assert.ThrowsAsync<UsernameTakenApiException>(
            async () => await userService.UpdateUserByField(session!.UserId, "email", new[] { "user@test.com" })
        );
    }
}