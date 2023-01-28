using System.Text.Json.Serialization;
using AutoMapper;
using DirectoryService.Api.Attributes;
using DirectoryService.Api.Controllers.V1.Models;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/user")]
[ApiController]
public sealed class UserController : V1ApiController
{
    private readonly UserService _userService;
    private readonly PlaceService _placeService;
    private readonly IMapper _mapper;

    public UserController(UserService userService,
        PlaceService placeService,
        IMapper mapper)
    {
        _userService = userService;
        _placeService = placeService;
        _mapper = mapper;
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
    /// Register a new place
    /// </summary>
    [HttpPost("places")]
    [Authorise]
    public async Task<IActionResult> RegisterPlace([FromBodyOrDefault] RegisterPlaceRootModel registerPlaceModel)
    {
        if (registerPlaceModel.Place is null)
            return Failure();

        var place = await _placeService.RegisterNewPlace(_mapper.Map<RegisterPlaceDto>(registerPlaceModel.Place));
        
        //TODO: Return PlaceInfo

        return Success();
    }

    /// <summary>
    /// Delete owned place
    /// </summary>
    [HttpDelete("places/{placeId:guid}")]
    [Authorise]
    public async Task<IActionResult> DeletePlace(Guid placeId)
    {
        await _placeService.DeletePlace(placeId);
        return Success();
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
        var page = PaginatedRequest("username", true, "username");
        page.Where.Add("connection", true);
        var result = await _userService.ListRelativeUsers(page);
        return Success(new UserListModel(result), result);
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
        var page = PaginatedRequest("username", true, "username");
        page.Where.Add("friend", true);
        var result = await _userService.ListRelativeUsers(page);
        return Success(new UserFriendsModel(result));
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
    public async Task<IActionResult> ReceiveHeartbeat(UserHeartbeatRootModel heartbeatModel)
    {
        var heartbeat = _mapper.Map<UserHeartbeatDto>(heartbeatModel.Location);
        await _userService.ProcessHeartbeat(heartbeat);
        return Success();
    }
    
    /// <summary>
    /// Receive a user's location
    /// </summary>
    [HttpPut("location")]
    [Authorise]
    public async Task<IActionResult> ReceiveLocation(UserHeartbeatRootModel locationHeartbeatModel)
    {
        var heartbeat = _mapper.Map<UserHeartbeatDto>(locationHeartbeatModel.Location);
        await _userService.ProcessHeartbeat(heartbeat);
        return Success();
    }

    /// <summary>
    /// Legacy get profile information
    /// </summary>
    [HttpGet("profile")]
    [Authorise]
    public async Task<IActionResult> GetUserProfile()
    {
        var profile = await _userService.GetUserProfile();

        return Success(new
        {
            User = new UserProfileModel()
            {
                AccountId = profile.UserId,
                Username = profile.Username,
                DiscourseApiKey = "",
                WalletId = Guid.Empty,
                XmppPassword = ""
            }
        });
    }

    public class UserProfileModel
    {
        [JsonPropertyName("accountId")]
        public Guid AccountId { get; set; }
        
        public string? Username { get; set; }
        public string? XmppPassword { get; set; }
        public string? DiscourseApiKey { get; set; }
        public Guid WalletId { get; set; }
    }

    /// <summary>
    /// Receive and process user's public key
    /// </summary>
    [HttpPut("public_key")]
    [Authorise]
    public async Task<IActionResult> PutPublicKey()
    {
        var cert = HttpContext.Request.Form.Files.GetFile("public_key");
        if (cert is null)
            throw new BaseApiException("FileMissing", "The file 'public_key' was not included.", 400);

        await _userService.UpdatePublicKey(cert.OpenReadStream());
        return Success();
    }

    // Does nothing for now since I believe the locker feature is deprecated
    [HttpPost("locker")]
    public IActionResult PostLocker()
    {
        return Success(new { });
    }
    
    // Does nothing for now since I believe the locker feature is deprecated
    [HttpGet("locker")]
    public IActionResult GetLocker()
    {
        return Success(new { });
    }
    
    
}