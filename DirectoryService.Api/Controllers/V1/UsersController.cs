using System.Net;
using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/users")]
[ApiController]
public sealed class UsersController : V1ApiController
{
    private readonly UserService _userService;
    
    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorise]
    public async Task<IActionResult> GetUsers()
    {
        var page = PaginatedRequest();
        
        var response = await _userService.ListUsers(page);
        
        return Success(response);
    }
    
    /// <summary>
    /// Request to register a new user
    /// </summary>
    /// <param name="registerUserModel"></param>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserModel registerUserModel)
    {
        if (registerUserModel.User == null)
            return Failure();

        var ip = HttpContext.Connection.RemoteIpAddress;
        var response = await _userService.RegisterUser(registerUserModel.User, ip ?? IPAddress.Parse("0.0.0.0"));
        
        return Success(response);
    }
    
    /// <summary>
    /// Exists purely because V1 of the API has the registration fields in a 'user' field
    /// </summary>
    public class RegisterUserModel
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public RegisterUserDto? User { get; set; }
    }

}