namespace Steam.Models.ManyTable;

public class UserNotifications
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }

    public int NotificationId { get; set; }
    public Notification Notification { get; set; }
}
