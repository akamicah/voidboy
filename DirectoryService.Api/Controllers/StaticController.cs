using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers;

[Route("static")]
public class StaticController : ControllerBase
{
    [HttpGet("email-verified")]
    public async Task<IActionResult> EmailVerifiedPage()
    {
        var html = await System.IO.File.ReadAllTextAsync(@"./static/email-verified.html");
        return base.Content(html, "text/html");
    }
    
    [HttpGet("verification-fail")]
    public async Task<IActionResult> EmailVerificationFail()
    {
        var html = await System.IO.File.ReadAllTextAsync(@"./static/email-verification-fail.html");
        return base.Content(html, "text/html");
    }
    
}