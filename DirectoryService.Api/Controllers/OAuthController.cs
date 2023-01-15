using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.Services;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace DirectoryService.Api.Controllers;

[Produces("application/json")]
[Route("oauth")]
[ApiController]
public sealed class OAuthController : ControllerBase
{ 
    private readonly OAuthService _service;
    private readonly IMapper _mapper;

    public OAuthController(OAuthService service,
        IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpPost("token")]
    [AllowAnonymous]
    public async Task<IActionResult> GrantToken([FromBodyOrDefault] TokenGrantRequestModel tokenGrantRequest)
    {
        var header = Response.Headers;
        try
        {
            var result = await _service.HandleGrantRequest(_mapper.Map<TokenGrantRequestDto>(tokenGrantRequest));
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

    [AutoMap(typeof(TokenGrantRequestDto))]
    public class TokenGrantRequestModel
    {
        [FromForm(Name = "grant_type")]
        public string? GrantType { get; set; }
        
        [FromForm(Name = "refresh_token")]
        public string? RefreshToken { get; set; }
        
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
    
    public class ModelMapperProfile : Profile
    {
        public ModelMapperProfile()
        {
            CreateMap<TokenGrantRequestModel, TokenGrantRequestDto>();
        }
    }
}