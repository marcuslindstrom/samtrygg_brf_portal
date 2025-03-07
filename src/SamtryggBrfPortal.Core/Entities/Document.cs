using SamtryggBrfPortal.Core.Enums;

namespace SamtryggBrfPortal.Core.Entities
{
    public class Document : BaseEntity
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DocumentType DocumentType { get; set; }
        public string UploadedBy { get; set; } = string.Empty;
        public bool RequiresSignature { get; set; }
        public bool IsSigned { get; set; }
        public DateTime? SignedDate { get; set; }
        public string? SignedBy { get; set; }
        public Guid? RentalApplicationId { get; set; }
        public RentalApplication? RentalApplication { get; set; }
        public ICollection<DocumentVersion> Versions { get; set; } = new List<DocumentVersion>();
    }
}