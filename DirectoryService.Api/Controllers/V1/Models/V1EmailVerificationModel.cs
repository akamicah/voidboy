using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Controllers.V1.Models;

/// <summary>
/// Model for V1 of the API
/// </summary>
public class V1EmailVerificationModel
{
    [FromQuery(Name = "a")]
    public string? AccountId { get; set; }
        
    [FromQuery(Name = "v")]
    public string? VerificationToken { get; set; }
}