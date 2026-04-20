using System.Net;

namespace ApiKit.Exceptions;

/// <summary>
/// Thrown when the API returns an HTTP 5xx response, indicating the service is
/// temporarily unable to handle the request. Defaults to
/// <see cref="HttpStatusCode.ServiceUnavailable"/>; pass the specific 5xx code
/// (e.g. <see cref="HttpStatusCode.InternalServerError"/>,
/// <see cref="HttpStatusCode.BadGateway"/>, <see cref="HttpStatusCode.GatewayTimeout"/>)
/// when known.
/// </summary>
/// <param name="message">Description of the error.</param>
/// <param name="innerException">Underlying cause, if any.</param>
/// <param name="statusCode">Specific 5xx status code. Defaults to <see cref="HttpStatusCode.ServiceUnavailable"/>.</param>
public sealed class APIServiceUnavailableException(
    string? message = null,
    Exception? innerException = null,
    HttpStatusCode statusCode = HttpStatusCode.ServiceUnavailable)
    : APIException(message, innerException, statusCode)
{
    /// <summary>
    /// Parameterless constructor for reflection-based creation and serializer defaults.
    /// </summary>
    public APIServiceUnavailableException() : this(null, null, HttpStatusCode.ServiceUnavailable)
    {
    }
}
