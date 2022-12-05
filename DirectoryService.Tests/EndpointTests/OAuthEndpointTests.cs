using System.Net;
using System.Net.Http.Json;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;

namespace DirectoryService.Tests.EndpointTests;

public class OAuthEndpointTests : TestBase
{
    [OneTimeSetUp]
    public void Setup()
    {
        TestSetup();
    }
    
    [Test]
    public async Task CanGrantPasswordToken()
    {
        var requestData = new
        {
            grant_type = "password",
            username = "test",
            password = "test123!"
        };
        
        var response = await _client.PostAsJsonAsync("api/oauth/token", requestData);

        var responseBody = await response.Content.ReadFromJsonAsync<GrantedTokenDto>();

        Assert.That(responseBody!, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(responseBody!.Scope, Is.EqualTo("owner"));
            Assert.That(responseBody!.TokenType, Is.EqualTo("Bearer"));
            Assert.That(responseBody!.AccountName, Is.EqualTo("test"));
            Assert.That(responseBody!.AccountRoles, Is.EqualTo(new List<string>() { "user" }));
        });
    }
}