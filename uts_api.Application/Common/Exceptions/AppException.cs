namespace uts_api.Application.Common.Exceptions;

public sealed class AppException : Exception
{
    public AppException(string messageKey, int statusCode = 400)
        : base(messageKey)
    {
        StatusCode = statusCode;
        MessageKey = messageKey;
    }

    public int StatusCode { get; }
    public string MessageKey { get; }
}
