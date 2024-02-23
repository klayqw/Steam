using Microsoft.AspNetCore.Identity;
using Steam.Models.ManyTable;

namespace Steam.Models;

public class User : IdentityUser
{
    public ICollection<UserGames> UserGames { get; set; }
    public ICollection<UserGroups> UserGroups { get; set; }
}
