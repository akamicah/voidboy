using DirectoryService.DAL.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Tests;

[TestFixture]
public class TestBase
{
    protected ApiWebApplicationFactory? _factory;
    protected HttpClient? _client;
    
    public void TestSetup()
    {
        _factory = new ApiWebApplicationFactory();
        _client = _factory.CreateClient();
        var dbContext = _factory.Services.GetRequiredService<DbContext>();
        
        dbContext.RunScript("truncateAll.sql");
        dbContext.RunScript("testSeed.sql");
    }
}