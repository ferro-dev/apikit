using System.Net;

#pragma warning disable CS1591

namespace ApiKit.Exceptions;

/// <summary>
/// Thrown when the API returns an HTTP 5xx response, indicating the service is
/// temporarily unable to handle the request. Defaults to
/// <see cref="HttpStatusCode.ServiceUnavailable"/>.
/// </summary>
public class APIServiceUnavailableException : APIException
{
    public APIServiceUnavailableException() : base(HttpStatusCode.ServiceUnavailable)
    {
    }

    public APIServiceUnavailableException(HttpStatusCode statusCode) : base(statusCode)
    {
    }

    public APIServiceUnavailableException(string? message) : base(HttpStatusCode.ServiceUnavailable, message)
    {
    }

    public APIServiceUnavailableException(HttpStatusCode statusCode, string? message) : base(statusCode, message)
    {
    }

    public APIServiceUnavailableException(string? message, Exception? innerException)
        : base(HttpStatusCode.ServiceUnavailable, message, innerException)
    {
    }

    public APIServiceUnavailableException(HttpStatusCode statusCode, string? message, Exception? innerException)
        : base(statusCode, message, innerException)
    {
    }
}
