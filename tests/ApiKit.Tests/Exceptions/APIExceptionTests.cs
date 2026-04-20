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
    public void Message_ctor_preserves_message_and_leaves_status_null()
    {
        var ex = new APIException("boom");

        ex.StatusCode.ShouldBeNull();
        ex.Message.ShouldBe("boom");
    }

    [Fact]
    public void Inner_ctor_preserves_inner_exception()
    {
        var inner = new InvalidOperationException("root cause");

        var ex = new APIException("boom", inner);

        ex.InnerException.ShouldBeSameAs(inner);
    }

    [Fact]
    public void Status_ctor_sets_status_code()
    {
        var ex = new APIException((HttpStatusCode)418);

        ex.StatusCode.ShouldBe((HttpStatusCode)418);
    }

    [Fact]
    public void Status_message_ctor_sets_both()
    {
        var ex = new APIException(HttpStatusCode.Conflict, "conflict");

        ex.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        ex.Message.ShouldBe("conflict");
    }

    [Fact]
    public void Status_message_inner_ctor_sets_all()
    {
        var inner = new InvalidOperationException("root");

        var ex = new APIException(HttpStatusCode.Conflict, "conflict", inner);

        ex.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        ex.Message.ShouldBe("conflict");
        ex.InnerException.ShouldBeSameAs(inner);
    }
}
