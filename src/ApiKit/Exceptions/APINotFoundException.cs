using System.Net;

namespace ApiKit.Exceptions;

/// <summary>
/// Thrown when the API returns an HTTP 404 Not Found response, indicating the
/// requested resource does not exist.
/// </summary>
/// <param name="message">Description of the error.</param>
/// <param name="innerException">Underlying cause, if any.</param>
public sealed class APINotFoundException(string? message = null, Exception? innerException = null)
    : APIException(message, innerException, HttpStatusCode.NotFound)
{
    /// <summary>
    /// Parameterless constructor for reflection-based creation and serializer defaults.
    /// </summary>
    public APINotFoundException() : this(null, null)
    {
    }
}
