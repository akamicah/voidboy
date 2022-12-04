namespace DirectoryService.Core.Exceptions;

[Serializable]
public class UserNotVerifiedApiException : BaseApiException
{
    public UserNotVerifiedApiException() : base("NotVerified", "User is not verified", 401)
    {
    }
}