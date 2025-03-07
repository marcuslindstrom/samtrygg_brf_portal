using SamtryggBrfPortal.Core.Enums;

namespace SamtryggBrfPortal.Core.Entities
{
    public class BackgroundCheck : BaseEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public Guid RentalApplicationId { get; set; }
        public RentalApplication RentalApplication { get; set; } = null!;
        public BackgroundCheckStatus Status { get; set; }
        public bool CreditCheckCompleted { get; set; }
        public bool IdentityVerified { get; set; }
        public bool ReferencesChecked { get; set; }
        public bool EmploymentVerified { get; set; }
        public bool HasSufficientIncome { get; set; }
        public string? Notes { get; set; }
        public string ReviewedBy { get; set; } = string.Empty;
        public DateTime? ReviewedAt { get; set; }
        public ICollection<BackgroundCheckDocument> Documents { get; set; } = new List<BackgroundCheckDocument>();
    }
}