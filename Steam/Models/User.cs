using Microsoft.AspNetCore.Identity;
using Steam.Models.ManyTable;

namespace Steam.Models;

public class User : IdentityUser
{
    public string AvatarUrl { get; set; }
    public ICollection<UserGames> UserGames { get; set; }
    public ICollection<UserGroups> UserGroups { get; set; }
    public ICollection<UserWorkShopSub> UserWorkShopSub { get; set; }
    public ICollection<UserNotifications> UserNotifications { get; set; }
    public ICollection<UserFriendship> Friendships { get; set; }
    public ICollection<GroupChat> GroupChats { get; set; }
}
