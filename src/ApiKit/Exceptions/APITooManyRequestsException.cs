using System.Net;
using System.Net.Http.Headers;

#pragma warning disable CS1591

namespace ApiKit.Exceptions;

/// <summary>
/// Thrown when the API returns an HTTP 429 Too Many Requests response. Inspect
/// <see cref="RetryAfter"/> to determine how long to wait before retrying, when
/// the server supplies a <c>Retry-After</c> header.
/// </summary>
public class APITooManyRequestsException : APIException
{
    public APITooManyRequestsException() : base(HttpStatusCode.TooManyRequests)
    {
    }

    public APITooManyRequestsException(TimeSpan? retryAfter) : base(HttpStatusCode.TooManyRequests)
    {
        RetryAfter = retryAfter;
    }

    public APITooManyRequestsException(string? message, TimeSpan? retryAfter = null)
        : base(HttpStatusCode.TooManyRequests, message)
    {
        RetryAfter = retryAfter;
    }

    public APITooManyRequestsException(string? message, Exception? innerException, TimeSpan? retryAfter = null)
        : base(HttpStatusCode.TooManyRequests, message, innerException)
    {
        RetryAfter = retryAfter;
    }

    /// <summary>
    /// Suggested duration to wait before retrying the request, parsed from the
    /// <c>Retry-After</c> response header. <see langword="null"/> when the header
    /// is absent or cannot be interpreted.
    /// </summary>
    public TimeSpan? RetryAfter { get; }

    /// <summary>
    /// Parses a <see cref="RetryConditionHeaderValue"/> into a <see cref="TimeSpan"/>.
    /// Supports delta-seconds (<c>Retry-After: 120</c>) and HTTP-date
    /// (<c>Retry-After: Wed, 21 Oct 2015 07:28:00 GMT</c>) forms. Returns
    /// <see langword="null"/> when the header is absent or the date has already
    /// passed and would yield a non-positive duration.
    /// </summary>
    /// <param name="header">The parsed <c>Retry-After</c> header value, or <see langword="null"/>.</param>
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
