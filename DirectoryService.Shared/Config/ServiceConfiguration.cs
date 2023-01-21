using System.Text.Json.Nodes;

namespace DirectoryService.Shared.Config;

public static class ServiceConfigurationContainer
{
    public static ServiceConfiguration Config { get; set; } = new ServiceConfiguration();
}

public class ServiceConfiguration
{
    public MetaverseInfoConfig? MetaverseInfo { get; set; }
    public DbConfig? Db { get; set; }
    public DirectoryServiceConfig? DirectoryService { get; set; }
    public RegistrationConfig? Registration { get; set; }
    public TokenConfig? Tokens { get; set; }
    public ServerConfig? Server { get; set; }
    public SmtpConfig? Smtp { get; set; }

    public class MetaverseInfoConfig
    {
        public string? Name { get; set; }
        public string? Nickname { get; set; }
        public string? ServerUrl { get; set; }
        public string? IceServerUrl { get; set; }
        public string? DashboardUrl { get; set; }
        public MetaverseVersion? MetaverseVersion { get; set; }
    }

    public class MetaverseVersion
    {
        public string? Version { get; set; }
        public string? Codename { get; set; }
    }
    
    public class DbConfig
    {
        public string? ConnectionString { get; set; }
    }
    
    public class DirectoryServiceConfig
    {
        public int MinDomainNameLength { get; set; }
        public int MaxDomainNameLength { get; set; }
        public int SessionTimeoutMinutes { get; set; }
        public int HeartbeatTimeoutSeconds { get; set; }
        public int DomainTimeoutSeconds { get; set; }
        public int DomainOnlineCheckSeconds { get; set; }
        public int FriendHandshakeTimeoutMinutes { get; set; }
        public int ConnectionRequestTimeoutMinutes { get; set; }
        public int FriendRequestTimeoutMinutes { get; set; }
        public int PlaceInformationTimeoutMinutes { get; set; }
        public int PlaceInactiveTimeoutMinutes { get; set; }
        public int PlaceActivityCheckSeconds { get; set; }
        public bool FixDomainNetworkAddress { get; set; }
        public bool AllowTempDomainCreation { get; set; }
        public string? TokenGenerationUrl { get; set; }
    }
    
    public class RegistrationConfig
    {
        public int MinUsernameLength { get; set; }
        public int MaxUsernameLength { get; set; }
        public bool RequireEmailVerification { get; set; }
        public int EmailVerificationTimeoutMinutes { get; set; }
        public string? EmailVerificationSuccessRedirect { get; set; }
        public string? EmailVerificationFailRedirect { get; set; }
        public string? DefaultAdminAccount { get; set; }
    }

    public class TokenConfig
    {
        public int DomainTokenLifetimeHours { get; set; }
        public int OwnerTokenLifetimeHours { get; set; }
    }

    public class ServerConfig
    {
        public int HttpPort { get; set; }
        public bool UseHttps { get; set; }
        public string? HttpsCertFile{ get; set; }
        public string? HttpsCertPassword { get; set; }
        public int HttpsPort { get; set; }
        public List<string>? KnownProxies { get; set; }
    }

    public class SmtpConfig
    {
        public bool Enable { get; set; }
        public string? SenderEmail { get; set; }
        public string? SenderName { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}