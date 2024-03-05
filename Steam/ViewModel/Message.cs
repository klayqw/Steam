using Steam.Models;

namespace Steam.ViewModel;

public class Message
{
    public User usersended { get; set; }
    public string message { get; set; }
    public DateTime date { get; set; }
}
