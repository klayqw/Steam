using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Steam.Dto;
public class GameDto
{
    public string GameImageUrl { get; set; }
    public string GamePreviewUrl { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Devoloper { get; set; }
    public string Publisher { get; set; }
    [JsonPropertyName("Price")]
    public double Price { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Genre { get; set; }
}

