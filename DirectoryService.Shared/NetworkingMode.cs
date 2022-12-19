namespace DirectoryService.Shared;

public enum NetworkingMode
{
    Invalid,
    Full,
    Ip,
    Disabled
}

public static class NetworkingModeExtension
{
    public static string ToNetworkingModeString(this NetworkingMode networkingMode)
    {
        return networkingMode switch
        {
            NetworkingMode.Full => "full",
            NetworkingMode.Ip => "ip",
            NetworkingMode.Disabled => "disabled",
            _ => "invalid"
        };
    }

    public static NetworkingMode ToNetworkingMode(this string? networkingMode)
    {
        if (networkingMode == null)
            return NetworkingMode.Invalid;

        return networkingMode.ToLower() switch
        {
            "full" => NetworkingMode.Full,
            "ip" => NetworkingMode.Ip,
            "disabled" => NetworkingMode.Disabled,
            _ => NetworkingMode.Invalid
        };
    }
}