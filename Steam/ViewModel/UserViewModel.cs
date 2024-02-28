using Steam.Models;

namespace Steam.ViewModel;

public class UserViewModel
{
    public User user { get; set; }
    public IEnumerable<Game> games { get; set; }
    public IEnumerable<Group> groups { get; set; }
}
