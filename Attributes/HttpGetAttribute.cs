using Turbo.az.Attributes.Base;

namespace Turbo.az.Attributes;
public class HttpGetAttribute : HttpAttribute
{
    public HttpGetAttribute(string routing) : base(HttpMethod.Get, routing) { }

    public HttpGetAttribute() : base(HttpMethod.Get, null) { }
}