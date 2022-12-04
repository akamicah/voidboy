namespace DirectoryService.Core.Exceptions;

public class InvalidTokenApiException : BaseApiException
{
    public InvalidTokenApiException() : base("InvalidToken", "The token provided is invalid.", 409)
    {
    }
}