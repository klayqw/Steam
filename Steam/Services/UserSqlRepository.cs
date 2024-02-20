using Dapper;
using Steam.Models;
using Steam.Services.Base;
using System.Data.SqlClient;

namespace Steam.Services;

public class UserSqlRepository : IUserRepositoryBase
{
    private readonly SqlConnection _connection;
    public UserSqlRepository(SqlConnection connection)
    {
        this._connection = connection;
    }
    public async Task AddAsync(User user)
    {
        user.Login = user.Login.ToLower();
        user.Password = user.Password.ToLower();
        user.Email = user.Email.ToLower();
        await _connection.ExecuteAsync("INSERT INTO Users ([Login], [Password], [Email]) VALUES(@Login, @Password, @Email);",
        new {
            user.Login,
            user.Password,
            user.Email,
        });
    }

    public async Task<User> FindAsync(string login, string password)
    {
        login = login.ToLower();
        password = password.ToLower();
        var result = await _connection.QueryFirstOrDefaultAsync<User>("select * from Users where Login=@Login and Password=@Password", new { Login = login, Password = password});
        return result;
    }

    public async Task<User> GetAsync(int id)
    {
        var result = await _connection.QueryFirstAsync<User>("SELECT u.*, g.* FROM Users u JOIN Games g ON u.GameId = g.Id WHERE u.Id = @Id;", new { Id = id });
        return result;
    }

    public async Task AddGameAsync(int gameid, int id)
    {
        var result = await _connection.ExecuteAsync("UPDATE Users SET GameId = @gameid WHERE Id = @id;", new {id , gameid});
        return;
    }
}
