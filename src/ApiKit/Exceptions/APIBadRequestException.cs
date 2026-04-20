using System.Net;

namespace ApiKit.Exceptions;

/// <summary>
/// Thrown when the API returns an HTTP 400 Bad Request response, typically indicating
/// a malformed request body or invalid parameters.
/// </summary>
/// <param name="message">Description of the error.</param>
/// <param name="innerException">Underlying cause, if any.</param>
public sealed class APIBadRequestException(string? message = null, Exception? innerException = null)
    : APIException(message, innerException, HttpStatusCode.BadRequest)
{
    /// <summary>
    /// Parameterless constructor for reflection-based creation and serializer defaults.
    /// </summary>
    public APIBadRequestException() : this(null, null)
    {
    }
}
