using System.Text.RegularExpressions;

namespace Steam.Models.ManyTable;

public class UserGames
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }

    public int GameId { get; set; }
    public Game Game { get; set; }
}
