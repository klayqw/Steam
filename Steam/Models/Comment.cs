namespace Steam.Models;

public class Comment
{
    public int Id { get; set; }
    public DateTime Time { get; set; } = DateTime.UtcNow;
    public string Text { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; }
}

