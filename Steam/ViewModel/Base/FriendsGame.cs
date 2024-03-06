using Steam.Models;

namespace Steam.ViewModel.Base;

public class FriendsGame
{
    public User user { get; set; }
    public IEnumerable<Game> games { get; set; }
}
