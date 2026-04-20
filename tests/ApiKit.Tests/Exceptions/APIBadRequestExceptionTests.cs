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
    public void Message_preserved_with_status()
    {
        var ex = new APIBadRequestException("bad");

        ex.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ex.Message.ShouldBe("bad");
    }

    [Fact]
    public void Inner_exception_preserved()
    {
        var inner = new Exception("root");

        var ex = new APIBadRequestException("bad", inner);

        ex.InnerException.ShouldBeSameAs(inner);
        ex.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public void Parameterless_ctor_usable_via_reflection()
    {
        var ex = (APIBadRequestException)Activator.CreateInstance(typeof(APIBadRequestException))!;

        ex.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
