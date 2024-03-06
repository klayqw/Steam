namespace Steam.ViewModel;

public class MessageViewModel
{
    public string creator {  get; set; }
    public int Groupid { get; set; }
    public IEnumerable<Message> messages { get; set; }
}
