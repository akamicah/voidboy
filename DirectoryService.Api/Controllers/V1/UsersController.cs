using System.Net;
using AutoMapper;
using DirectoryService.Api.Attributes;
using DirectoryService.Api.Controllers.V1.Models;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Services;
using DirectoryService.Shared;
using Microsoft.AspNetCore.Mvc;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/users")]
[ApiController]
public sealed class UsersController : V1ApiController
{
    private readonly UserService _userService;
    private readonly IMapper _mapper;
    
    public UsersController(UserService userService,
        IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
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
        return Success(new UserListModel(result));
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
    /// <param name="registerUserModel"></param>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBodyOrDefault] RegisterUserRootModel registerUserModel)
    {
        if (registerUserModel.User == null)
            return Failure();
        
        var registerUser = _mapper.Map<RegisterUserDto>(registerUserModel.User);
        
        var ip = HttpContext.Connection.RemoteIpAddress;
        registerUser.OriginIp = ip ?? IPAddress.Any;
        
        var response = await _userService.RegisterUser(registerUser);
        
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