
namespace ConsoleApp1.Attribute;

public class HttpDelete : HttpAttributeBase
{
    public HttpDelete(string routing) : base(HttpMethod.Delete, routing) { }
    public HttpDelete() : base(HttpMethod.Delete, null) { }
}
