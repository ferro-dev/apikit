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
    public void Named_status_overrides_default(HttpStatusCode status)
    {
        new APIServiceUnavailableException(statusCode: status).StatusCode.ShouldBe(status);
    }

    [Fact]
    public void Message_preserved_with_default_status()
    {
        var ex = new APIServiceUnavailableException("down");

        ex.StatusCode.ShouldBe(HttpStatusCode.ServiceUnavailable);
        ex.Message.ShouldBe("down");
    }

    [Fact]
    public void All_params_preserved()
    {
        var inner = new Exception("root");

        var ex = new APIServiceUnavailableException("boom", inner, HttpStatusCode.InternalServerError);

        ex.Message.ShouldBe("boom");
        ex.InnerException.ShouldBeSameAs(inner);
        ex.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public void Parameterless_ctor_usable_via_reflection()
    {
        var ex = (APIServiceUnavailableException)Activator.CreateInstance(typeof(APIServiceUnavailableException))!;

        ex.StatusCode.ShouldBe(HttpStatusCode.ServiceUnavailable);
    }
}
