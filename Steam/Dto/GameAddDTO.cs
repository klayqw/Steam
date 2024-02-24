using System.ComponentModel.DataAnnotations;

namespace Steam.Dto;
public class GameAddDTO
{
    public string GameImageUrl { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Devoloper { get; set; }
    public string Publisher { get; set; }
    public double Price { get; set; }
    public DateTime ReleaseDate { get; set; }
    [Required]
    public string Genre { get; set; }
}

