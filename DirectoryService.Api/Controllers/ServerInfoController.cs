using DirectoryService.Api.Attributes;
using DirectoryService.Api.Middleware.Authentication;
using DirectoryService.Shared.Config;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable global IdentifierTypo
// ReSharper disable global StringLiteralTypo

namespace DirectoryService.Api.Controllers;

[Produces("application/json")]
[Route("api")]
[ApiController]
public class ServerInfoController : ControllerBase
{
    private readonly ServiceConfiguration _configuration;
    
    public ServerInfoController()
    {
        _configuration = ServicesConfigContainer.Config;
    }
    
    [HttpGet("metaverse_info")]
    [HttpGet("v1/metaverse_info")]
    [AllowAnonymous]
    public async Task<IActionResult> GetMetaverseInfo()
    {
        return new JsonResult(new MetaverseInfoModel()
        {
            MetaverseName = _configuration.MetaverseInfo.Name,
            MetaverseNickName = _configuration.MetaverseInfo.Nickname,
            IceServerUrl = _configuration.MetaverseInfo.IceServerUrl,
            MetaverseServerVersion = new MetaverseVersionModel()
            {
                Project = "Void Boy"
            },
            MetaverseUrl = _configuration.MetaverseInfo.ServerUrl
        });
    }

    public class MetaverseVersionModel
    {
        public string? Project { get; set; }
    }
    
    public class MetaverseInfoModel
    {
        public string? MetaverseName { get; set; }
        public string? MetaverseNickName { get; set; }
        public string? MetaverseUrl { get; set; }
        public string? IceServerUrl { get; set; }
        public MetaverseVersionModel?  MetaverseServerVersion { get; set; }
    }
    
}