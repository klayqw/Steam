using Dapper;
using StackOverflow.Models;
using System.Data.SqlClient;

namespace StackOverflow.Services;

public class LogSqlServices : ILogRepository
{
    private readonly string connection;
    public LogSqlServices(string connection)
    {
        this.connection = connection;
    }
    public async Task AddLog(LogInfo logInfo)
    {
        using SqlConnection connection = new SqlConnection(this.connection);
        try
        {
            var result = await connection.ExecuteAsync("INSERT INTO [Log] (userid, url, method_type, status_code, request_body, response_body) VALUES (@userid, @url, @methodtype, @statuscode, @request_body, @response_body)",
            new { logInfo.userid, logInfo.url, logInfo.methodtype, logInfo.statuscode, logInfo.request_body, logInfo.response_body });
        }catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        
       
    }
}
