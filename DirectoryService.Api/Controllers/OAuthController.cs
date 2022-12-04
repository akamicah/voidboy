using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.Services;

namespace DirectoryService.Api.Controllers;

[Produces("application/json")]
[Route("api/oauth")]
[ApiController]
public sealed class OAuthController : ControllerBase
{ 
    private readonly OAuthService _service;

    public OAuthController(OAuthService service)
    {
        _service = service;
    }

    [HttpPost("token")]
    [AllowAnonymous]
    public async Task<IActionResult> Grant([FromBody] TokenGrantRequestDto grantRequest)
    {
        try
        {
            var result = await _service.HandleGrantRequest(grantRequest);
            return new JsonResult(result);
        }
        catch (InvalidCredentialsApiException)
        {
            return new JsonErrorResult(new { error = "InvalidCredentials" }, HttpStatusCode.Unauthorized);
        }
        catch (UserNotVerifiedApiException)
        {
            return new JsonErrorResult(new { error = "UserNotVerified" }, HttpStatusCode.Unauthorized);
        }
        catch (ArgumentException e)
        {
            return new JsonErrorResult(new { error = e.Message }, HttpStatusCode.BadRequest);
        }
    }
}