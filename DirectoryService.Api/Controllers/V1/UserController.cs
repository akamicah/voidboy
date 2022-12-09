using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/user")]
[ApiController]
public sealed class UserController : V1ApiController
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }
    

    /// <summary>
    /// Get requester owned places
    /// </summary>
    [HttpGet("places")]
    [Authorise]
    public async Task<IActionResult> GetUserPlaces()
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// Redirect to proper location
    /// </summary>
    // TODO: Is this even necessary anymore?
    [HttpGet("/user/places")]
    [Authorise]
    public async Task<IActionResult> GetPlacesRedirect()
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// Fetch information for owned place
    /// </summary>
    [HttpGet("places/{placeId:guid}")]
    [Authorise]
    public async Task<IActionResult> GetUserPlace(Guid placeId)
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// Delete owned place
    /// </summary>
    [HttpDelete("places/{placeId:guid}")]
    [Authorise]
    public async Task<IActionResult> DeleteUserPlace(Guid placeId)
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// Legacy - Update owned place
    /// </summary>
    [HttpPut("places/{placeId:guid}")]
    [Authorise]
    public async Task<IActionResult> UpdateUserPlace(Guid placeId)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Connection request
    /// </summary>
    [HttpPost("connection_request")]
    [Authorise]
    public async Task<IActionResult> RequestUserConnection()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Delete connection request
    /// </summary>
    [HttpPost("connection_request")]
    [Authorise]
    public async Task<IActionResult> RemoveUserConnectionRequest()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Fetch user connections for logged in account
    /// </summary>
    [HttpGet("connections")]
    [Authorise]
    public async Task<IActionResult> GetUserConnections()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Upgrade a connection to a friend (not implemented)
    /// </summary>
    [HttpPost("connections")]
    [Authorise]
    public async Task<IActionResult> UpgradeUserConnection()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Remove a connection from the requester connections list
    /// </summary>
    [HttpDelete("connections/{username}")]
    [Authorise]
    public async Task<IActionResult> RemoveUserConnection(string username)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Return a list of the requester friends
    /// </summary>
    [HttpGet("friends")]
    [Authorise]
    public async Task<IActionResult> GetFriends()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Upgrade a requester connection to friend
    /// </summary>
    [HttpPost("friends")]
    [Authorise]
    public async Task<IActionResult> UserConnectionToFriend()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Remove a friend from the requester friends list
    /// </summary>
    [HttpDelete("friends/{username}")]
    [Authorise]
    public async Task<IActionResult> RemoveFriend(string username)
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// Receive a user's heartbeat
    /// </summary>
    [HttpPut("heartbeat")]
    [Authorise]
    public async Task<IActionResult> ReceiveHeartbeat()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Receive a user's location
    /// </summary>
    [HttpPut("location")]
    [Authorise]
    public async Task<IActionResult> ReceiveLocation()
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// Legacy get profile information
    /// </summary>
    [HttpGet("profile")]
    [Authorise]
    public async Task<IActionResult> GetUserProfile()
    {
        //TODO
        throw new NotImplementedException();
    }

    /// <summary>
    /// Receive and process user's public key
    /// </summary>
    [HttpPut("public_key")]
    [Authorise]
    public async Task<IActionResult> PutPublicKey()
    {
        //TODO
        throw new NotImplementedException();
    }
}