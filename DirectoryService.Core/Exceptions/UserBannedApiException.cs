namespace DirectoryService.Core.Exceptions;

[Serializable]
public class UserBannedApiException : BaseApiException
{
    public UserBannedApiException() : base("Banned", "Banned User", 401)
    {
    }
}