using System.Net;
using ApiKit.Exceptions;
using Shouldly;
using Xunit;

namespace ApiKit.Tests.Exceptions;

public class APIBadRequestExceptionTests
{
    [Fact]
    public void Has_status_400()
    {
        new APIBadRequestException().StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public void Is_APIException()
    {
        new APIBadRequestException().ShouldBeAssignableTo<APIException>();
    }

    [Fact]
    public void Message_ctor_preserves_status_400()
    {
        var ex = new APIBadRequestException("bad");

        ex.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ex.Message.ShouldBe("bad");
    }

    [Fact]
    public void Message_inner_ctor_preserves_all()
    {
        var inner = new Exception("root");

        var ex = new APIBadRequestException("bad", inner);

        ex.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ex.Message.ShouldBe("bad");
        ex.InnerException.ShouldBeSameAs(inner);
    }
}
