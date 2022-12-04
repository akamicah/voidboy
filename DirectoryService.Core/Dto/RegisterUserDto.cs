namespace DirectoryService.Core.Dto;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable once ClassNeverInstantiated.Global

public class RegisterUserDto
{
    public string? Username { get; set; }
    public string? Password { get; set;}
    public string? Email { get; set;}
}