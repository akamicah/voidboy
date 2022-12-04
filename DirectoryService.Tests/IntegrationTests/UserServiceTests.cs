using DirectoryService.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Tests.IntegrationTests;

[TestFixture]
public class UserServiceTests
{
    private ApiWebApplicationFactory _factory;

    [OneTimeSetUp]
    public void Setup()
    {
        _factory = new ApiWebApplicationFactory();
    }

    [Test]
    public async Task CanRegisterUserTest()
    {
        var userService = _factory.Services.GetRequiredService<UserService>();

        //TODO
    }
    
    
}