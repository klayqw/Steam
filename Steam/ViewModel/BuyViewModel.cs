using Steam.Models;

namespace Steam.ViewModel;

public class BuyViewModel
{
    public Game game { get; set; }
    public IEnumerable<Game> UserGames { get; set;}
    public IEnumerable<User> Friends { get; set; }
}
