using DirectoryService.Core.Dto;
using DirectoryService.Shared;

namespace DirectoryService.Api.Controllers.V1.Models;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

public class UserListModel
{
    public UserListModel(PaginatedResult<UserSearchResultDto> result)
    {
        Users = result.Data!;
    }
    public IEnumerable<UserSearchResultDto> Users { get; set; }
}