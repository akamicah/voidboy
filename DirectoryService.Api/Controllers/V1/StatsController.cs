using DirectoryService.Api.Attributes;
using DirectoryService.Api.Controllers.V1.Models;
using DirectoryService.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/stats")]
[ApiController]
public class StatsController : V1ApiController
{
    [Authorise]
    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        // TODO
        return Success(new StatsListModel());
    }
    
    [Authorise]
    [HttpGet("stat/{stat}")]
    public async Task<IActionResult> GetStat(string stat, [FromQuery] bool includeHistory = false)
    {
        // TODO
        return Success(new
        {
            Stat = new {}
        });
    }
    
    [Authorise]
    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetCategory(string category, [FromQuery] bool includeHistory = false)
    {
        // TODO
        return Success(new
        {
        });
    }
}