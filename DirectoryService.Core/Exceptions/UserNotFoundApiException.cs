namespace DirectoryService.Core.Exceptions;

[Serializable]
public class UserNotFoundApiException : BaseApiException
{
    public UserNotFoundApiException() : base("UserNotFound", "User was not found", 400)
    {
    }
}