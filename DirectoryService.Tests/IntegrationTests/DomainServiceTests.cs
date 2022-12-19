using System.Net;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Tests.IntegrationTests;

[TestFixture]
public class DomainServiceTests : TestBase
{
    [OneTimeSetUp]
    public void Setup()
    {
        TestSetup();
    }

    [Test]
    public async Task CanRegisterDomain()
    {
        var domainService = _factory!.Services.GetRequiredService<DomainService>();

        var registerDomain = new RegisterDomainDto()
        {
            Name = "MyDomain",
            NetworkAddress = "127.0.0.1",
            NetworkPort = 12345,
            OriginIp = IPAddress.Any
        };

        var domain = await domainService.RegisterNewDomain(registerDomain);
        
        Assert.That(domain,Is.Not.Null);
    }
}