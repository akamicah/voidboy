using DirectoryService.Core.Dto;
using DirectoryService.Shared;

namespace DirectoryService.Api.Controllers.V1.Models;

public class V1UserListModel
{
    public V1UserListModel(PaginatedResult<UserSearchResultDto> result)
    {
        Users = result.Data!;
    }
    public IEnumerable<UserSearchResultDto> Users { get; set; }
}