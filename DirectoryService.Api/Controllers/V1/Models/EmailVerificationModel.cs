using Microsoft.AspNetCore.Mvc;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DirectoryService.Api.Controllers.V1.Models;

public class EmailVerificationModel
{
    [FromQuery(Name = "a")]
    public string? AccountId { get; set; }
        
    [FromQuery(Name = "v")]
    public string? VerificationToken { get; set; }
}