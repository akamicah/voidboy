using System.Text.Json.Serialization;
using DirectoryService.Api.Attributes;
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
        _configuration = ServiceConfigurationContainer.Config;
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
            MetaverseServerVersion = _configuration.MetaverseInfo.MetaverseVersion,
            MetaverseUrl = _configuration.MetaverseInfo.ServerUrl
        });
    }

    public class MetaverseInfoModel
    {
        [JsonPropertyName("metaverse_name")] 
        public string? MetaverseName { get; set; }
        
        [JsonPropertyName("metaverse_nick_name")] 
        public string? MetaverseNickName { get; set; }
        
        [JsonPropertyName("metaverse_url")] 
        public string? MetaverseUrl { get; set; }
        
        [JsonPropertyName("ice_server_url")] 
        public string? IceServerUrl { get; set; }

        [JsonPropertyName("metaverse_server_version")]
        public ServiceConfiguration.MetaverseVersion? MetaverseServerVersion { get; set; }
    }
}