using System.Net;
using System.Net.Http.Headers;

namespace ApiKit.Exceptions;

/// <summary>
/// Thrown when the API returns an HTTP 429 Too Many Requests response. Inspect
/// <see cref="RetryAfter"/> to determine how long to wait before retrying, when
/// the server supplied a <c>Retry-After</c> header.
/// </summary>
/// <param name="message">Description of the error.</param>
/// <param name="innerException">Underlying cause, if any.</param>
/// <param name="retryAfter">Duration to wait before retrying, parsed from the <c>Retry-After</c> header.</param>
public sealed class APITooManyRequestsException(
    string? message = null,
    Exception? innerException = null,
    TimeSpan? retryAfter = null)
    : APIException(message, innerException, HttpStatusCode.TooManyRequests)
{
    /// <summary>
    /// Parameterless constructor for reflection-based creation and serializer defaults.
    /// </summary>
    public APITooManyRequestsException() : this(null, null, null)
    {
    }

    /// <summary>
    /// Suggested duration to wait before retrying the request, parsed from the
    /// <c>Retry-After</c> response header. <see langword="null"/> when the header
    /// is absent or cannot be interpreted.
    /// </summary>
    public TimeSpan? RetryAfter { get; } = retryAfter;

    /// <summary>
    /// Parses a <see cref="RetryConditionHeaderValue"/> into a <see cref="TimeSpan"/>.
    /// Supports delta-seconds (<c>Retry-After: 120</c>) and HTTP-date
    /// (<c>Retry-After: Wed, 21 Oct 2015 07:28:00 GMT</c>) forms. Returns
    /// <see langword="null"/> when the header is absent; returns
    /// <see cref="TimeSpan.Zero"/> when the HTTP-date has already passed.
    /// </summary>
    /// <param name="header">Parsed <c>Retry-After</c> header value, or <see langword="null"/>.</param>
    /// <param name="now">Reference time for HTTP-date calculations. Defaults to <see cref="DateTimeOffset.UtcNow"/>.</param>
    public static TimeSpan? ParseRetryAfter(RetryConditionHeaderValue? header, DateTimeOffset? now = null)
    {
        if (header is null)
            return null;

        if (header.Delta is { } delta)
            return delta;

        if (header.Date is { } date)
        {
            var reference = now ?? DateTimeOffset.UtcNow;
            var remaining = date - reference;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }

        return null;
    }
}
