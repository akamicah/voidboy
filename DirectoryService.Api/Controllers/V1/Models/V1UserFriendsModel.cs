using DirectoryService.Core.Dto;
using DirectoryService.Shared;

namespace DirectoryService.Api.Controllers.V1.Models;

public class V1UserFriendsModel
{
    public V1UserFriendsModel(PaginatedResult<UserSearchResultDto> result)
    {
        Friends = result.Data?.Select(x => x.Username!).ToList() ?? new List<string>();
    }
    public List<string> Friends { get; set; }
}