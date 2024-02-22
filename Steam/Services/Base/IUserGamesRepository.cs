using Steam.Models;

namespace Steam.Services.Base;

public interface IUserGamesRepository
{
    public Task<IEnumerable<Game>> GetUserGames(int id);
    public Task AddGameToUser(int userId, int gameId);
}
