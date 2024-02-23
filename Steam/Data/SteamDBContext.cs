using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Steam.Models;
using Steam.Models.ManyTable;

namespace Steam.Data;

public class SteamDBContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Notification> notifications { get; set; }
    public DbSet<WorkShop> workShops { get; set; }
    public DbSet<UserGames> userGames { get; set; }
    public DbSet<UserGroups> userGroups { get; set; }
    public DbSet<UserNotifications> userNotifications { get; set; }
    public DbSet<UserWorkShopSub> userWorkShopSubs { get; set; }

    public SteamDBContext(DbContextOptions<SteamDBContext> options) : base(options) { }

  
}
