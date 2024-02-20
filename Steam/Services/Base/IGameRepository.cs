using Steam.Models;

namespace Steam.Services.Base;

public interface IGameRepository
{
    public Task Post(Game game);
    public Task<Game> GetById(int id);
    public Task<IEnumerable<Game>> GetAll();
}
