namespace DirectoryService.Core.Exceptions;

[Serializable]
public class InvalidTokenApiException : BaseApiException
{
    public InvalidTokenApiException() : base("InvalidToken", "The token provided is invalid.", 409)
    {
    }
}