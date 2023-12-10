
namespace ConsoleApp1.Attribute;

internal class HttpPost : HttpAttributeBase
{
    public HttpPost(string routing) : base(HttpMethod.Post, routing) { }
    public HttpPost() : base(HttpMethod.Post, null) { }

}
