using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackOverflow.Models;
using System.Data.SqlClient;

namespace StackOverflow.Services;

public class ForumSqlRepository : IForumRepository
{
    public readonly string sqlconnection;

    public ForumSqlRepository(string sqlconnection)
    {
        this.sqlconnection = sqlconnection;
    }

    public async Task AddAsync(Forum forum)
    {
        using SqlConnection connection = new SqlConnection(sqlconnection);
        var post = await connection.ExecuteAsync("insert into Forum(Title,[Description],[Like],Dislike) values(@Title,@Description,@Like,@Dislike)", new { forum.Title, forum.Description, forum.Like, forum.Dislike });
    }

    public async Task DeleteAsync(int id)
    {
        using SqlConnection connection = new SqlConnection(sqlconnection);
        var delete = await connection.ExecuteAsync("delete from Forum where Id = @id", new {id});
    }

    public async Task<Forum> GetAsync(int id)
    {
        using SqlConnection connection = new SqlConnection(sqlconnection);

        var forum = await connection.QueryFirstOrDefaultAsync<Forum>("select * from Forum where Id = @id", new { id });

        return forum;
    }

    public async Task UpdateAsync(Forum forum, int id)
    {
        using SqlConnection connection = new SqlConnection(sqlconnection);
        var update = await connection.ExecuteAsync("update Forum set Title = @Title,[Description]=@Description,[Like]=@Like,Dislike=@Dislike where Id = @id", new { forum.Title, forum.Description, forum.Like, forum.Dislike,id});
    }

    public async Task<List<Forum>> GetAllAsync()
    {
        using SqlConnection connection = new SqlConnection(sqlconnection);

        var forum = await connection.QueryAsync<Forum>("select * from Forum");

        return forum.ToList();
    }
}
