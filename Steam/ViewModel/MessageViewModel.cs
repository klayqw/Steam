namespace Steam.ViewModel;

public class MessageViewModel
{
    public int Groupid { get; set; }
    public IEnumerable<Message> messages { get; set; }
}
