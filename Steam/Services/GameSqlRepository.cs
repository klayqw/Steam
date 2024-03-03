
using Dapper;
using Steam.Models;
using Steam.Services.Base;
using System.Data.SqlClient;

namespace Steam.Services;

public class GameSqlRepository : IGameRepository
{
    private readonly SqlConnection sqlConnection;
    public GameSqlRepository(SqlConnection sqlConnection) 
    {
        this.sqlConnection = sqlConnection;
    }

    public async Task<IEnumerable<Game>> GetAll()
    {
        var result = await sqlConnection.QueryAsync<Game>("select * from Games");
        return result;
    }

    public async Task<Game> GetById(int id)
    {
        var result = await sqlConnection.QueryFirstOrDefaultAsync<Game>("select * from Games where Id = @id" ,new { id });
        return result;
    }

    public async Task Post(Game game)
    {
        await sqlConnection.ExecuteAsync(@"INSERT INTO Games ([Title], [Description], [Devoloper], [Publisher], [Price], [ReleaseDate], [Genre]) 
        VALUES (@Title, @Description, @Devoloper, @Publisher,@Price, @ReleaseDate, @Genre)", 
        new {
            game.Title,
            game.Description,
            game.Devoloper,
            game.Publisher,
            game.Price,
            game.ReleaseDate,
            game.Genre,
        });
    }
}
