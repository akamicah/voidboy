namespace DirectoryService.Core.Exceptions;

[Serializable]
public class UserSuspendedApiException : BaseApiException
{
    public UserSuspendedApiException() : base("Suspended", "Suspended User", 401)
    {
    }
}