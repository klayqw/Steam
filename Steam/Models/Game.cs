namespace Steam.Models;

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Devoloper { get; set; }
    public string Publisher { get; set; }
    public double Price { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Genre { get; set; }

}
