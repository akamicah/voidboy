namespace DirectoryService.Core.Exceptions;

[Serializable]
public class InvalidCredentialsApiException : BaseApiException
{
    public InvalidCredentialsApiException() : base("InvalidCredentials", "Incorrect username/password.", 401)
    {
    }
}