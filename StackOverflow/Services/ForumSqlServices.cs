using Dapper;
using Microsoft.Extensions.Options;
using StackOverflow.Models;
using System.Data.SqlClient;

namespace StackOverflow.Services;

public class ForumSqlRepository : IForumRepository
{
    public readonly string sqlconnection;

    public ForumSqlRepository(string sqlconnection)
    {
        Console.WriteLine(sqlconnection);
        this.sqlconnection = sqlconnection;
    }

    public async Task AddAsync(Forum forum)
    {

    }

    public async Task DeleteAsync(Forum forum)
    {

    }

    public async Task<Forum> GetAsync(int id)
    {
        using SqlConnection connection = new SqlConnection(sqlconnection);

        var forum = await connection.QueryFirstOrDefaultAsync<Forum>("select * from Forum where Id = @id", new { id });

        return forum;
    }

    public async Task UpdateAsync(Forum forum)
    {
       
    }

    public async Task<IEnumerable<Forum>> GetAllForumsAsync()
    {
        using SqlConnection connection = new SqlConnection(sqlconnection);

        var forum = await connection.QueryFirstOrDefaultAsync<IEnumerable<Forum>>("select * from Forum");

        return forum;
    }
}
