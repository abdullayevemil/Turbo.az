using Turbo.az.Attributes.Base;

namespace Turbo.az.Attributes;
public class HttpPostAttribute : HttpAttribute
{
    public HttpPostAttribute(string routing) : base(HttpMethod.Post, routing) { }

    public HttpPostAttribute() : base(HttpMethod.Post, null) { }
}