using System.Net;
using System.Net.Http.Headers;
using ApiKit.Exceptions;
using Shouldly;
using Xunit;

namespace ApiKit.Tests.Exceptions;

public class APITooManyRequestsExceptionTests
{
    [Fact]
    public void Has_status_429()
    {
        new APITooManyRequestsException().StatusCode.ShouldBe(HttpStatusCode.TooManyRequests);
    }

    [Fact]
    public void Is_APIException()
    {
        new APITooManyRequestsException().ShouldBeAssignableTo<APIException>();
    }

    [Fact]
    public void RetryAfter_preserved_via_named_arg()
    {
        var ex = new APITooManyRequestsException(retryAfter: TimeSpan.FromSeconds(30));

        ex.RetryAfter.ShouldBe(TimeSpan.FromSeconds(30));
        ex.StatusCode.ShouldBe(HttpStatusCode.TooManyRequests);
    }

    [Fact]
    public void All_params_preserved()
    {
        var inner = new Exception("root");

        var ex = new APITooManyRequestsException("slow down", inner, TimeSpan.FromSeconds(5));

        ex.Message.ShouldBe("slow down");
        ex.InnerException.ShouldBeSameAs(inner);
        ex.RetryAfter.ShouldBe(TimeSpan.FromSeconds(5));
        ex.StatusCode.ShouldBe(HttpStatusCode.TooManyRequests);
    }

    [Fact]
    public void Parameterless_ctor_usable_via_reflection()
    {
        var ex = (APITooManyRequestsException)Activator.CreateInstance(typeof(APITooManyRequestsException))!;

        ex.StatusCode.ShouldBe(HttpStatusCode.TooManyRequests);
        ex.RetryAfter.ShouldBeNull();
    }

    [Fact]
    public void ParseRetryAfter_returns_null_when_header_missing()
    {
        APITooManyRequestsException.ParseRetryAfter(null).ShouldBeNull();
    }

    [Fact]
    public void ParseRetryAfter_delta_seconds()
    {
        var header = new RetryConditionHeaderValue(TimeSpan.FromSeconds(120));

        APITooManyRequestsException.ParseRetryAfter(header).ShouldBe(TimeSpan.FromSeconds(120));
    }

    [Fact]
    public void ParseRetryAfter_http_date()
    {
        var now = new DateTimeOffset(2026, 4, 20, 12, 0, 0, TimeSpan.Zero);
        var target = now.AddSeconds(90);
        var header = new RetryConditionHeaderValue(target);

        var parsed = APITooManyRequestsException.ParseRetryAfter(header, now);

        parsed.ShouldBe(TimeSpan.FromSeconds(90));
    }

    [Fact]
    public void ParseRetryAfter_http_date_in_past_returns_zero()
    {
        var now = new DateTimeOffset(2026, 4, 20, 12, 0, 0, TimeSpan.Zero);
        var past = now.AddSeconds(-30);
        var header = new RetryConditionHeaderValue(past);

        APITooManyRequestsException.ParseRetryAfter(header, now).ShouldBe(TimeSpan.Zero);
    }

    [Fact]
    public void ParseRetryAfter_round_trip_via_HttpResponseMessage_delta()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
        response.Headers.TryAddWithoutValidation("Retry-After", "60").ShouldBeTrue();

        var parsed = APITooManyRequestsException.ParseRetryAfter(response.Headers.RetryAfter);

        parsed.ShouldBe(TimeSpan.FromSeconds(60));
    }
}
