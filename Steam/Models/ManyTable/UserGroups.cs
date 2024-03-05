using System.ComponentModel.DataAnnotations;

namespace Steam.Models.ManyTable;

public class UserGroups
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }

    public int GroupId { get; set; }
    public Group Group { get; set; }
}
