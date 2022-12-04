namespace DirectoryService.Core.Exceptions;

public class UsernameTakenApiException : BaseApiException
{
    public UsernameTakenApiException() : base("UsernameTaken", "An account is already registered to this username.", 409)
    {
    }
}