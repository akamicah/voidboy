namespace DirectoryService.Core.Exceptions;

[Serializable]
public class DomainNotFoundApiException : BaseApiException
{
    public DomainNotFoundApiException() : base("DomainNotFound", "Domain was not found", 404)
    {
    }
}