using System.Net;
using DirectoryService.Api.Attributes;
using DirectoryService.Api.Controllers.V1.Models;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Services;
using DirectoryService.Shared;
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

    /// <summary>
    /// Return a list of users relative to the requester.
    /// </summary>
    [HttpGet]
    [Authorise]
    public async Task<IActionResult> GetUsers()
    {
        var page = PaginatedRequest("username", true, "username");
        var result = await _userService.ListRelativeUsers(page);
        return Success(new V1UserListModel(result));
    }

    /// <summary>
    /// Redirect to dashboard user's profile
    /// </summary>
    // TODO: Is this even necessary anymore?
    [HttpGet("/users/{username}")]
    [Authorise]
    public async Task<IActionResult> GetUserRedirect(string username)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Request to register a new user
    /// </summary>
    /// <param name="v1RegisterUserModel"></param>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] V1RegisterUserModel registerUserModel)
    {
        if (registerUserModel.User == null)
            return Failure();

        var ip = HttpContext.Connection.RemoteIpAddress;
        registerUserModel.User.OriginIp = ip ?? IPAddress.Any;
        var response = await _userService.RegisterUser(registerUserModel.User);
        
        return Success(response);
    }

    /// <summary>
    /// Fetch a user's location
    /// </summary>
    [HttpGet("{accountId:guid}/location")]
    [Authorise]
    public async Task<IActionResult> GetUserLocation(Guid accountId)
    {
        //TODO
        throw new NotImplementedException();
    }
}