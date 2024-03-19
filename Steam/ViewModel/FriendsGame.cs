using Steam.Models;

namespace Steam.ViewModel;

public class FriendsGame
{
    public User user { get; set; }
    public IEnumerable<Game> games { get; set; }
}
