using System.Net;
using ApiKit.Exceptions;
using Shouldly;
using Xunit;

namespace ApiKit.Tests.Exceptions;

public class APIExceptionTests
{
    [Fact]
    public void Default_ctor_has_null_status()
    {
        var ex = new APIException();

        ex.StatusCode.ShouldBeNull();
        ex.Message.ShouldNotBeNull();
    }

    [Fact]
    public void Message_preserved_and_status_defaults_null()
    {
        var ex = new APIException("boom");

        ex.StatusCode.ShouldBeNull();
        ex.Message.ShouldBe("boom");
    }

    [Fact]
    public void Inner_exception_preserved()
    {
        var inner = new InvalidOperationException("root cause");

        var ex = new APIException("boom", inner);

        ex.InnerException.ShouldBeSameAs(inner);
    }

    [Fact]
    public void Status_code_preserved_via_named_arg()
    {
        var ex = new APIException(statusCode: HttpStatusCode.Conflict);

        ex.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }

    [Fact]
    public void All_params_preserved()
    {
        var inner = new InvalidOperationException("root");

        var ex = new APIException("conflict", inner, HttpStatusCode.Conflict);

        ex.Message.ShouldBe("conflict");
        ex.InnerException.ShouldBeSameAs(inner);
        ex.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }
}
