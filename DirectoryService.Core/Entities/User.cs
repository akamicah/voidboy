using DirectoryService.Shared;

namespace DirectoryService.Core.Entities;

public class User : GuidIdentifiedEntity
{
    public IdentityProvider IdentityProvider;
    public AccountState State;
    public string? Username;
    public string? Email;
    public string? AuthHash;
    public long AuthVersion;
    public bool Activated;
    public UserRole Role;
    public string? CreatorIp;
    public string? Language;
}