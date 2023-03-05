using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.Core.Dto;

public class UpdateUserDto
{
    public Guid Id { get; set; }
    
    [EditableField("username", new [] { Permission.Owner, Permission.Admin })]
    public string? Username { get; set; }
    
    [EditableField("email", new [] { Permission.Owner, Permission.Admin })]
    public string? Email { get; set; }
    
    [EditableField("account_settings", new [] { Permission.Owner, Permission.Admin })]
    public string? AccountSettings { get; set; }
    
    [EditableField("images_hero", new [] { Permission.Owner, Permission.Admin })]
    public string? ImagesHero { get; set; }
    
    [EditableField("images_tiny", new [] { Permission.Owner, Permission.Admin })]
    public string? ImagesTiny { get; set; }
    
    [EditableField("images_thumbnail", new [] { Permission.Owner, Permission.Admin })]
    public string? ImagesThumbnail { get; set; }
    
    [EditableField("availability", new [] { Permission.Owner, Permission.Admin })]
    public string? Availability { get; set; }
    
    [EditableField("connections", new [] { Permission.Owner, Permission.Admin })]
    public string[]? Connections { get; set; }
    
    [EditableField("friends", new [] { Permission.Owner, Permission.Admin })]
    public string[]? Friends { get; set; }
    
    [EditableField("password", new [] { Permission.Owner, Permission.Admin })]
    public string? Password;
    
    [EditableField("public_key", new [] { Permission.Owner, Permission.Admin })]
    public string? PublicKey;
    
    [EditableField("roles", new [] { Permission.Admin })]
    public string[]? Roles;
    
    [EditableField("ip_addr_of_creator", new [] { Permission.None })]
    public string? IpAddressOfCreator { get; set; }
}