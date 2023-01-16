namespace DirectoryService.Core.Exceptions;

[Serializable]
public class PlaceNotFoundApiException : BaseApiException
{
    public PlaceNotFoundApiException() : base("PlaceNotFound", "Place was not found", 400)
    {
    }
}