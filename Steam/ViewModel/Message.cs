using Steam.Models;

namespace Steam.ViewModel;

public class Message
{
    public int Id { get; set; }
    public User usersended { get; set; }
    public string message { get; set; }
    public DateTime date { get; set; }
}
