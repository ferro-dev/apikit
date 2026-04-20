using System.Net;

#pragma warning disable CS1591

namespace ApiKit.Exceptions;

/// <summary>
/// Thrown when the API returns an HTTP 401 Unauthorized or 403 Forbidden response,
/// indicating that the caller is not authenticated or lacks permission to perform
/// the requested action. Defaults to <see cref="HttpStatusCode.Forbidden"/>.
/// </summary>
public class APIForbiddenException : APIException
{
    public APIForbiddenException() : base(HttpStatusCode.Forbidden)
    {
    }

    public APIForbiddenException(HttpStatusCode statusCode) : base(statusCode)
    {
    }

    public APIForbiddenException(string? message) : base(HttpStatusCode.Forbidden, message)
    {
    }

    public APIForbiddenException(HttpStatusCode statusCode, string? message) : base(statusCode, message)
    {
    }

    public APIForbiddenException(string? message, Exception? innerException)
        : base(HttpStatusCode.Forbidden, message, innerException)
    {
    }

    public APIForbiddenException(HttpStatusCode statusCode, string? message, Exception? innerException)
        : base(statusCode, message, innerException)
    {
    }
}
