using System.ComponentModel.DataAnnotations;

namespace Steam.Models.ManyTable
{
    public class GroupChat
    {
        public int Id { get; set; }

        [Required]
        public string MessageContent { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
