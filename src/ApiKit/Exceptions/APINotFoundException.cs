using System.Net;

#pragma warning disable CS1591

namespace ApiKit.Exceptions;

/// <summary>
/// Thrown when the API returns an HTTP 404 Not Found response, indicating the
/// requested resource does not exist.
/// </summary>
public class APINotFoundException : APIException
{
    public APINotFoundException() : base(HttpStatusCode.NotFound)
    {
    }

    public APINotFoundException(string? message) : base(HttpStatusCode.NotFound, message)
    {
    }

    public APINotFoundException(string? message, Exception? innerException)
        : base(HttpStatusCode.NotFound, message, innerException)
    {
    }
}
