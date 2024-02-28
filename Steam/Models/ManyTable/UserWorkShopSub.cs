namespace Steam.Models.ManyTable;

public class UserWorkShopSub
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }

    public int WorkShopItemId { get; set; }
    public WorkShop WorkShopItem { get; set; }
}
