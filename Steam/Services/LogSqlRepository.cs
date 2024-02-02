using Dapper;
using Steam.Models;
using Steam.Services.Base;
using System.Data.SqlClient;

namespace Steam.Services
{
    public class LogSqlRepository : ILogRepository
    {
        private readonly SqlConnection Connection;

        public LogSqlRepository(SqlConnection connection)
        {
            this.Connection = connection;
        }

        public async Task Add(Log log)
        {
            await Connection.ExecuteAsync(
                @"INSERT INTO Logs ([UserId], [Url], [MethodType], [StatusCode], [RequestBody], [ResponseBody])
                values(@UserId,@Url,@MethodType,@StatusCode,@RequestBody,@ResponseBody)",
                new {
                    log.UserId,
                    log.Url,
                    log.MethodType,
                    log.StatusCode,
                    log.ResponseBody,
                    log.RequestBody,
                });

        }
    }
}
