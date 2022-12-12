namespace DirectoryService.Core.Exceptions;

[Serializable]
public class EmailNotAvailableApiException : BaseApiException
{
    public EmailNotAvailableApiException() : base("EmailServiceNotAvailable", "Unable to complete request as e-mail not available.", 501)
    {
    }
}