using System.Net;
using ApiKit.Exceptions;
using Shouldly;
using Xunit;

namespace ApiKit.Tests.Exceptions;

public class APINotFoundExceptionTests
{
    [Fact]
    public void Has_status_404()
    {
        new APINotFoundException().StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public void Is_APIException()
    {
        new APINotFoundException().ShouldBeAssignableTo<APIException>();
    }

    [Fact]
    public void Message_ctor_preserves_status_404()
    {
        var ex = new APINotFoundException("missing");

        ex.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        ex.Message.ShouldBe("missing");
    }

    [Fact]
    public void Message_inner_ctor_preserves_all()
    {
        var inner = new Exception("root");

        var ex = new APINotFoundException("missing", inner);

        ex.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        ex.InnerException.ShouldBeSameAs(inner);
    }
}
