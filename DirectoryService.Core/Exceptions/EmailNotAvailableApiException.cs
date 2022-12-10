namespace DirectoryService.Core.Exceptions;

public class EmailNotAvailableApiException : BaseApiException
{
    public EmailNotAvailableApiException() : base("EmailServiceNotAvailable", "Unable to complete request as e-mail not available.", 501)
    {
    }
}