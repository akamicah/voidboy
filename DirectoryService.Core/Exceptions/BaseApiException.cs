namespace DirectoryService.Core.Exceptions;

[Serializable]
public class BaseApiException : Exception
{
    private string _errorCode;
    private string _errorMessage;
    private int _statusCode;

    public BaseApiException(string code, string errorMessage, int statusCode = 200) : base(errorMessage)
    {
        _errorCode = code;
        _errorMessage = errorMessage;
        _statusCode = statusCode;
    }

    public string ApiErrorCode()
    {
        return _errorCode;
    }

    public string ApiErrorMessage()
    {
        return _errorMessage;
    }
    
    public int ApiStatusCode()
    {
        return _statusCode;
    }
}