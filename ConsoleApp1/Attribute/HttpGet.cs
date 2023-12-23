

namespace ConsoleApp1.Attribute;

public class HttpGet : HttpAttributeBase
{
    public HttpGet(string routing) : base(HttpMethod.Get, routing) { }
    public HttpGet() : base(HttpMethod.Get, null) { }
}
