using System.Net;
using ApiKit.Exceptions;
using Shouldly;
using Xunit;

namespace ApiKit.Tests.Exceptions;

public class APIServiceUnavailableExceptionTests
{
    [Fact]
    public void Default_has_status_503()
    {
        new APIServiceUnavailableException().StatusCode.ShouldBe(HttpStatusCode.ServiceUnavailable);
    }

    [Fact]
    public void Is_APIException()
    {
        new APIServiceUnavailableException().ShouldBeAssignableTo<APIException>();
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    [InlineData(HttpStatusCode.GatewayTimeout)]
    public void Explicit_status_overrides_default(HttpStatusCode status)
    {
        new APIServiceUnavailableException(status).StatusCode.ShouldBe(status);
    }

    [Fact]
    public void Message_ctor_keeps_default_status()
    {
        var ex = new APIServiceUnavailableException("down");

        ex.StatusCode.ShouldBe(HttpStatusCode.ServiceUnavailable);
        ex.Message.ShouldBe("down");
    }

    [Fact]
    public void Status_message_ctor_covers_502()
    {
        var ex = new APIServiceUnavailableException(HttpStatusCode.BadGateway, "gateway");

        ex.StatusCode.ShouldBe(HttpStatusCode.BadGateway);
        ex.Message.ShouldBe("gateway");
    }

    [Fact]
    public void Message_inner_ctor_preserves_all()
    {
        var inner = new Exception("root");

        var ex = new APIServiceUnavailableException("down", inner);

        ex.StatusCode.ShouldBe(HttpStatusCode.ServiceUnavailable);
        ex.InnerException.ShouldBeSameAs(inner);
    }

    [Fact]
    public void Status_message_inner_ctor_preserves_all()
    {
        var inner = new Exception("root");

        var ex = new APIServiceUnavailableException(HttpStatusCode.InternalServerError, "boom", inner);

        ex.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        ex.InnerException.ShouldBeSameAs(inner);
    }
}
