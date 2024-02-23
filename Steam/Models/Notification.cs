using System.ComponentModel.DataAnnotations;

namespace Steam.Models;

public class Notification
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int UserTo { get; set; }
    [Required]
    public int UserFrom { get; set; }
    [Required]
    public string Type {  get; set; }

}
