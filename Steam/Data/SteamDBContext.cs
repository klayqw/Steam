using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Steam.Models;
using Steam.Models.ManyTable;

namespace Steam.Data;

public class SteamDBContext : IdentityDbContext<User, IdentityRole, string>
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Notification> notifications { get; set; }
    public DbSet<WorkShop> workShops { get; set; }
    public DbSet<UserGames> userGames { get; set; }
    public DbSet<UserGroups> userGroups { get; set; }
    public DbSet<UserNotifications> userNotifications { get; set; }
    public DbSet<UserWorkShopSub> userWorkShopSubs { get; set; }
    public DbSet<UserFriendship> Friendships { get; set; }
    public DbSet<GroupChat> GroupChat { get; set; }
    public DbSet<Comment> Comment { get; set; }

    public SteamDBContext(DbContextOptions<SteamDBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserFriendship>()
            .HasKey(uf => uf.Id); 

        modelBuilder.Entity<UserFriendship>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.Friendships)
            .HasForeignKey(uf => uf.UserId) 
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserFriendship>()
            .HasOne(uf => uf.Friend)
            .WithMany()
            .HasForeignKey(uf => uf.FriendId) 
            .OnDelete(DeleteBehavior.Restrict);
    }
}
