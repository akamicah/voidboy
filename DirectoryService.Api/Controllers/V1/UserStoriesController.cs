using DirectoryService.Api.Attributes;
using DirectoryService.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1;

public sealed class UserStoriesController : V1ApiController
{
    /// <summary>
    /// Fetch user stories
    /// </summary>
    // TODO: Investigate what constitutes as a story
    [HttpGet]
    [Authorise]
    public async Task<IActionResult> GetUserStories()
    {
        //TODO
        throw new NotImplementedException();
    }
}