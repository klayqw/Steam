using Dapper;
using Steam.Models;
using Steam.Services.Base;
using System.Data.SqlClient;

namespace Steam.Services;

public class UserGameRepository : IUserGamesRepository
{
    private readonly SqlConnection _connection;
    public UserGameRepository(SqlConnection connection1)
    {
        _connection = connection1;
    }

    public async Task AddGameToUser(int userId, int gameId)
    {
        var count = await _connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM UsersGames WHERE UserId = @UserId AND GameId = @GameId", new { UserId = userId, GameId = gameId });
        if(count > 0)
        {
            throw new Exception("You allready have this game!");
        }
        await _connection.ExecuteAsync("INSERT INTO UsersGames (UserId, GameId) VALUES (@userId, @gameId)", new { userId, gameId });
    }

    public async Task<IEnumerable<Game>> GetUserGames(int id)
    {
        var result = await _connection.QueryAsync<Game>("SELECT g.* FROM Users u JOIN UsersGames ug ON u.Id = ug.UserId JOIN Games g ON ug.GameId = g.Id WHERE u.Id = @id", new {id});

        return result;
    }
}
