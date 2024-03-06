using Steam.Models;
using Steam.ViewModel.Base;

namespace Steam.ViewModel;

public class LibaryViewModel
{
    public IEnumerable<Game> games { get; set; }
    public IEnumerable<FriendsGame> friends { get; set; }
}
