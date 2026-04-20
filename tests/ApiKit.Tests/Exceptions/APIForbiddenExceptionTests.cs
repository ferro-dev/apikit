using System.Net;
using ApiKit.Exceptions;
using Shouldly;
using Xunit;

namespace ApiKit.Tests.Exceptions;

public class APIForbiddenExceptionTests
{
    [Fact]
    public void Default_has_status_403()
    {
        new APIForbiddenException().StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public void Is_APIException()
    {
        new APIForbiddenException().ShouldBeAssignableTo<APIException>();
    }

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    public void Explicit_status_overrides_default(HttpStatusCode status)
    {
        new APIForbiddenException(status).StatusCode.ShouldBe(status);
    }

    [Fact]
    public void Message_ctor_keeps_default_status_and_preserves_message()
    {
        var ex = new APIForbiddenException("nope");

        ex.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        ex.Message.ShouldBe("nope");
    }

    [Fact]
    public void Status_message_ctor_covers_401()
    {
        var ex = new APIForbiddenException(HttpStatusCode.Unauthorized, "auth");

        ex.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        ex.Message.ShouldBe("auth");
    }

    [Fact]
    public void Message_inner_ctor_preserves_all()
    {
        var inner = new Exception("root");

        var ex = new APIForbiddenException("nope", inner);

        ex.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        ex.InnerException.ShouldBeSameAs(inner);
    }

    [Fact]
    public void Status_message_inner_ctor_preserves_all()
    {
        var inner = new Exception("root");

        var ex = new APIForbiddenException(HttpStatusCode.Unauthorized, "auth", inner);

        ex.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        ex.InnerException.ShouldBeSameAs(inner);
    }
}
