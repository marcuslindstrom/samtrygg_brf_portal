namespace SamtryggBrfPortal.Core.Entities
{
    public class Message : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public string SenderUserId { get; set; } = string.Empty;
        public string RecipientUserId { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public Guid? RentalApplicationId { get; set; }
        public RentalApplication? RentalApplication { get; set; }
    }
}