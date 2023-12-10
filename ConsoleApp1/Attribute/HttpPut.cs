
namespace ConsoleApp1.Attribute;

public class HttpPut : HttpAttributeBase
{
    public HttpPut(string routing) : base(HttpMethod.Put, routing) { }
    public HttpPut() : base(HttpMethod.Put,null) { }
}
