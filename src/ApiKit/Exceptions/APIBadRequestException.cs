using System.Net;

#pragma warning disable CS1591

namespace ApiKit.Exceptions;

/// <summary>
/// Thrown when the API returns an HTTP 400 Bad Request response, typically
/// indicating a malformed request body or invalid parameters.
/// </summary>
public class APIBadRequestException : APIException
{
    public APIBadRequestException() : base(HttpStatusCode.BadRequest)
    {
    }

    public APIBadRequestException(string? message) : base(HttpStatusCode.BadRequest, message)
    {
    }

    public APIBadRequestException(string? message, Exception? innerException)
        : base(HttpStatusCode.BadRequest, message, innerException)
    {
    }
}
