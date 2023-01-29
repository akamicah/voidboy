using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

/// <summary>
/// Endpoint methods just to prevent 404 errors. Deprecated feature no longer in use
/// </summary>
[Produces("application/json")]
[Route("api/v1/commerce")]
[ApiController]
public sealed class CommerceController : V1ApiController
{
    [HttpGet("available_updates")]
    [Authorise]
    public async Task<IActionResult> GetAvailableUpdates()
    {
        return Success();
    }
    
    [HttpGet("history")]
    [Authorise]
    public async Task<IActionResult> GetHistory()
    {
        return Success();
    }
    
    [HttpGet("marketplace_keys")]
    [Authorise]
    public async Task<IActionResult> GetMarketplaceKey()
    {
        return Success();
    }
    
    [HttpPut("hfc_account")]
    [Authorise]
    public async Task<IActionResult> PutHfcAccount()
    {
        return Success();
    }
}