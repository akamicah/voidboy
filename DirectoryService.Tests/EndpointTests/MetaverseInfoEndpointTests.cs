using System.Net;
using System.Net.Http.Json;
using DirectoryService.Api.Controllers;
using DirectoryService.Shared.Config;

namespace DirectoryService.Tests.EndpointTests;

[TestFixture]
public class MetaverseInfoEndpointTests
{
    private ApiWebApplicationFactory? _factory;
    private HttpClient? _client;
    private ServiceConfiguration? _config;
    
    [OneTimeSetUp]
    public void Setup()
    {
        _factory = new ApiWebApplicationFactory();
        _client = _factory.CreateClient();
        _config = ServicesConfigContainer.Config;
    }

    [Test]
    public async Task CanGetMetaverseInformation()
    {
        var response = await _client.GetAsync("api/metaverse_info");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var info = await response.Content.ReadFromJsonAsync<ServerInfoController.MetaverseInfoModel>();
        
        Assert.That(info, Is.Not.Null);
        
        Assert.Multiple(() =>
        {
            Assert.That(info!.MetaverseName, Is.EqualTo(_config!.MetaverseInfo.Name));
            Assert.That(info!.MetaverseUrl, Is.EqualTo(_config!.MetaverseInfo.ServerUrl));
            Assert.That(info!.IceServerUrl, Is.EqualTo(_config!.MetaverseInfo.IceServerUrl));
            Assert.That(info!.MetaverseNickName, Is.EqualTo(_config!.MetaverseInfo.Nickname));
        });
    }

    [Test]
    public async Task CanGetMetaverseInformationV1()
    {
        var response = await _client.GetAsync("api/v1/metaverse_info");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var info = await response.Content.ReadFromJsonAsync<ServerInfoController.MetaverseInfoModel>();
        
        Assert.That(info, Is.Not.Null);
        
        Assert.Multiple(() =>
        {
            Assert.That(info!.MetaverseName, Is.EqualTo(_config!.MetaverseInfo.Name));
            Assert.That(info!.MetaverseUrl, Is.EqualTo(_config!.MetaverseInfo.ServerUrl));
            Assert.That(info!.IceServerUrl, Is.EqualTo(_config!.MetaverseInfo.IceServerUrl));
            Assert.That(info!.MetaverseNickName, Is.EqualTo(_config!.MetaverseInfo.Nickname));
        });
    }
}