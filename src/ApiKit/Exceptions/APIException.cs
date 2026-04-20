using System.Net;

// Ctors are thin wrappers mirroring System.Exception; per-ctor summaries would be noise.
#pragma warning disable CS1591

namespace ApiKit.Exceptions;

/// <summary>
/// Base exception type for errors returned by an HTTP API. Carries the originating
/// <see cref="HttpStatusCode"/>. Derived types represent specific status codes
/// (<see cref="APIBadRequestException"/>, <see cref="APIForbiddenException"/>,
/// <see cref="APINotFoundException"/>, <see cref="APITooManyRequestsException"/>,
/// <see cref="APIServiceUnavailableException"/>).
/// </summary>
/// <remarks>
/// This type does not carry a typed error body. Consumers that need to surface a
/// deserialized error payload should attach it via <see cref="Exception.Data"/> or
/// derive their own exception type.
/// </remarks>
public class APIException : Exception
{
    public APIException()
    {
    }

    public APIException(string? message) : base(message)
    {
    }

    public APIException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public APIException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public APIException(HttpStatusCode statusCode, string? message) : base(message)
    {
        StatusCode = statusCode;
    }

    public APIException(HttpStatusCode statusCode, string? message, Exception? innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    /// <summary>
    /// HTTP status code associated with the failing response, when known.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }
}
