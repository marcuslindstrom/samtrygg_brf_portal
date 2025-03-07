using SamtryggBrfPortal.Core.Enums;

namespace SamtryggBrfPortal.Core.Entities
{
    public class RentalApplication : BaseEntity
    {
        public Guid PropertyId { get; set; }
        public Property Property { get; set; } = null!;
        public string OwnerId { get; set; } = string.Empty;
        public string? TenantId { get; set; }
        public string ApplicantFirstName { get; set; } = string.Empty;
        public string ApplicantLastName { get; set; } = string.Empty;
        public string ApplicantEmail { get; set; } = string.Empty;
        public string ApplicantUserId { get; set; } = string.Empty;
        public decimal MonthlyRent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RentalReason { get; set; } = string.Empty;
        public RentalStatus Status { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public string? RejectionReason { get; set; }
        public string? LastStatusChangeByUserId { get; set; }
        public DateTime? LastStatusChangeAt { get; set; }
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public BackgroundCheck? BackgroundCheck { get; set; }
        
        public string ApplicantName => $"{ApplicantFirstName} {ApplicantLastName}";
    }
}
