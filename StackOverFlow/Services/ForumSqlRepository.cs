using Dapper;
using StackOverFlow.Models;
using StackOverFlow.Services.Base;
using System.Data.SqlClient;

namespace StackOverFlow.Services;

public class ForumSqlRepository : IForumRepository
{
    private readonly string Connection;

    public ForumSqlRepository(string Connection)
    {
        this.Connection = Connection;
    }
    public async Task AddAsync(Forum forum)
    {
        using SqlConnection sqlConnection = new SqlConnection(Connection);
        try
        {
            await sqlConnection.ExecuteAsync("insert into Forum(Title,[Description],[Like],Dislike) values(@Title,@Description,@Like,@Dislike)", new { forum.Title, forum.Description, forum.Like, forum.Dislike });
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IEnumerable<Forum>> GetAll()
    {
        using SqlConnection sqlConnection = new SqlConnection(Connection);
        IEnumerable<Forum> result;
        try
        {
            result = await sqlConnection.QueryAsync<Forum>("select * from Forum");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }
}
