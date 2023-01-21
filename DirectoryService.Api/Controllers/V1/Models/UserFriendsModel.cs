using DirectoryService.Core.Dto;
using DirectoryService.Shared;

namespace DirectoryService.Api.Controllers.V1.Models;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

public class UserFriendsModel
{
    public UserFriendsModel(PaginatedResult<UserSearchResultDto> result)
    {
        Friends = result.Data?.Select(x => x.Username!).ToList() ?? new List<string>();
    }
    public List<string> Friends { get; set; }
}