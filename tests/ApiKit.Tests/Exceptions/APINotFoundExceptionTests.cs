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
    public void Message_preserved_with_status()
    {
        var ex = new APINotFoundException("missing");

        ex.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        ex.Message.ShouldBe("missing");
    }

    [Fact]
    public void Inner_exception_preserved()
    {
        var inner = new Exception("root");

        var ex = new APINotFoundException("missing", inner);

        ex.InnerException.ShouldBeSameAs(inner);
        ex.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public void Parameterless_ctor_usable_via_reflection()
    {
        var ex = (APINotFoundException)Activator.CreateInstance(typeof(APINotFoundException))!;

        ex.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
