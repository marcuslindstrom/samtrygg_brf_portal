using SamtryggBrfPortal.Core.Enums;

namespace SamtryggBrfPortal.Core.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string RecipientUserId { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public NotificationType Type { get; set; }
        public string? ActionLink { get; set; }
        public Guid? RentalApplicationId { get; set; }
        public RentalApplication? RentalApplication { get; set; }
    }
}