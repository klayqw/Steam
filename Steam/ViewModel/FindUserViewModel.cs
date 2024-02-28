using Steam.Models;

namespace Steam.ViewModel;

public class FindUserViewModel
{
    public IEnumerable<User> users { get; set; }
    public User currentUser { get; set; }
}
