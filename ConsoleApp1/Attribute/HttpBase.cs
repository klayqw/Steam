
namespace ConsoleApp1.Attribute;

public abstract class HttpAttributeBase : System.Attribute
{
    public readonly HttpMethod MethodType;
    private readonly string? routing;
    public string? NormalizedRouting => routing?.Trim('/').ToLower();

    public HttpAttributeBase(HttpMethod methodType, string? routing)
    {
        this.routing = routing;
        this.MethodType = methodType;
    }
}