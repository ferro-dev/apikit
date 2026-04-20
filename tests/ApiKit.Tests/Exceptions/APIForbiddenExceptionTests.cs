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
    public void Named_status_overrides_default(HttpStatusCode status)
    {
        new APIForbiddenException(statusCode: status).StatusCode.ShouldBe(status);
    }

    [Fact]
    public void Message_preserved_with_default_status()
    {
        var ex = new APIForbiddenException("nope");

        ex.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        ex.Message.ShouldBe("nope");
    }

    [Fact]
    public void All_params_preserved()
    {
        var inner = new Exception("root");

        var ex = new APIForbiddenException("auth", inner, HttpStatusCode.Unauthorized);

        ex.Message.ShouldBe("auth");
        ex.InnerException.ShouldBeSameAs(inner);
        ex.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Parameterless_ctor_usable_via_reflection()
    {
        var ex = (APIForbiddenException)Activator.CreateInstance(typeof(APIForbiddenException))!;

        ex.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
