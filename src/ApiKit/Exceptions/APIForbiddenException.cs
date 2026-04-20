using System.Net;

namespace ApiKit.Exceptions;

/// <summary>
/// Thrown when the API returns an HTTP 401 Unauthorized or 403 Forbidden response,
/// indicating that the caller is not authenticated or lacks permission to perform
/// the requested action. Defaults to <see cref="HttpStatusCode.Forbidden"/>; pass
/// <see cref="HttpStatusCode.Unauthorized"/> explicitly for 401 responses.
/// </summary>
/// <param name="message">Description of the error.</param>
/// <param name="innerException">Underlying cause, if any.</param>
/// <param name="statusCode">Either <see cref="HttpStatusCode.Forbidden"/> (default) or <see cref="HttpStatusCode.Unauthorized"/>.</param>
public sealed class APIForbiddenException(
    string? message = null,
    Exception? innerException = null,
    HttpStatusCode statusCode = HttpStatusCode.Forbidden)
    : APIException(message, innerException, statusCode)
{
    /// <summary>
    /// Parameterless constructor for reflection-based creation and serializer defaults.
    /// </summary>
    public APIForbiddenException() : this(null, null, HttpStatusCode.Forbidden)
    {
    }
}
