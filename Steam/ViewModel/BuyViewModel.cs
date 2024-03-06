using Steam.Models;

namespace Steam.ViewModel;

public class BuyViewModel
{
    public Game game { get; set; }
    public IEnumerable<Game> UserGames { get; set;}
    public IEnumerable<Comment> Comments { get; set;}
}
