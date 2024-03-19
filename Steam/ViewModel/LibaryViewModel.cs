using Steam.Models;

namespace Steam.ViewModel;

public class LibaryViewModel
{
    public IEnumerable<Game> games { get; set; }
    public IEnumerable<FriendsGame> friends { get; set; }
}
