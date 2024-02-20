namespace Steam.Models;

public class Log
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Url { get; set; }
    public string MethodType { get; set; }
    public string StatusCode { get; set; }
    public string RequestBody { get; set; }
    public string ResponseBody { get; set; }
}
