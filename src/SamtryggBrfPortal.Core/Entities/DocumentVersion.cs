namespace SamtryggBrfPortal.Core.Entities
{
    public class DocumentVersion : BaseEntity
    {
        public Guid DocumentId { get; set; }
        public Document Document { get; set; } = null!;
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public int VersionNumber { get; set; }
        public string? ChangeDescription { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}