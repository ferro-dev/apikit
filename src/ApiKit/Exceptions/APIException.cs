using System.Net;

namespace ApiKit.Exceptions;

/// <summary>
/// Base exception type for errors returned by an HTTP API. Carries the originating
/// <see cref="HttpStatusCode"/> when known. Derived types model specific status
/// classes (<see cref="APIBadRequestException"/>, <see cref="APIForbiddenException"/>,
/// <see cref="APINotFoundException"/>, <see cref="APITooManyRequestsException"/>,
/// <see cref="APIServiceUnavailableException"/>). Throw this type directly only when
/// the response status is not modeled by a derived type.
/// </summary>
/// <remarks>
/// This type intentionally does not carry a typed error body. Consumers that need to
/// surface a deserialized error payload should attach it via <see cref="Exception.Data"/>
/// or derive their own exception type.
/// </remarks>
/// <param name="message">Description of the error.</param>
/// <param name="innerException">Underlying cause, if any.</param>
/// <param name="statusCode">HTTP status code associated with the failing response, when known.</param>
public class APIException(
    string? message = null,
    Exception? innerException = null,
    HttpStatusCode? statusCode = null)
    : Exception(message, innerException)
{
    /// <summary>
    /// Parameterless constructor for reflection-based creation (e.g.
    /// <see cref="Activator.CreateInstance(Type)"/>) and serializer defaults.
    /// </summary>
    public APIException() : this(null, null, null)
    {
    }

    /// <summary>
    /// HTTP status code associated with the failing response, when known.
    /// </summary>
    public HttpStatusCode? StatusCode { get; } = statusCode;
}
