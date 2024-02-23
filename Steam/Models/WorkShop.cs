using System.ComponentModel.DataAnnotations;

namespace Steam.Models;

public class WorkShop
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int Like {  get; set; }
    [Required]
    public int Dislike {  get; set; }
    [Required]
    public int Subscribers { get; set; }
}
