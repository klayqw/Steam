using Steam.Models.ManyTable;
using System.ComponentModel.DataAnnotations;

namespace Steam.Models;

public class Game
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Devoloper { get; set; }
    [Required]
    public string Publisher { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public DateTime ReleaseDate { get; set; }
    [Required]
    public string Genre { get; set; }
    public ICollection<UserGames> UserGames { get; set; }
    public ICollection<Comment> CommentGames { get; set; }
}
