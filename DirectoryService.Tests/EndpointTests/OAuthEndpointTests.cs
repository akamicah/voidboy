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
            username = "user",
            password = "password"
        };
        
        var response = await _client.PostAsJsonAsync("oauth/token", requestData);

        var responseBody = await response.Content.ReadFromJsonAsync<GrantedTokenDto>();

        Assert.That(responseBody!, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(responseBody!.Scope, Is.EqualTo("owner"));
            Assert.That(responseBody!.TokenType, Is.EqualTo("Bearer"));
            Assert.That(responseBody!.AccountName, Is.EqualTo("user"));
            Assert.That(responseBody!.AccountRoles, Is.EqualTo(new List<string>() { "user" }));
        });
    }
}