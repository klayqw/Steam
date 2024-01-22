namespace StackOverflow.Models;

public class LogInfo
{
    public int logid {get;set;}
    public int userid { get; set; }
    public string url { get; set; }
    public string methodtype {get;set;}
	public int statuscode { get; set; }
    public string request_body { get; set; }
    public string response_body { get; set; }

}
