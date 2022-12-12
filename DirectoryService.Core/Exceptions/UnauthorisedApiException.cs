namespace DirectoryService.Core.Exceptions;

[Serializable]
public class UnauthorisedApiException : BaseApiException
{
    public UnauthorisedApiException() : base("Unauthorised", "You are not authorised to do this.", 401)
    {
    }
}