namespace DirectoryService.Core.Exceptions;

[Serializable]
public class EmailAddressTakenApiException : BaseApiException
{
    public EmailAddressTakenApiException() : base("EmailAlreadyInUse", "An account is already registered to this e-mail address.", 409)
    {
    }
}