namespace SamtryggBrfPortal.Core.Entities
{
    public class BrfDocument : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string UploadedBy { get; set; } = string.Empty;
        public Guid BrfAssociationId { get; set; }
        public BrfAssociation BrfAssociation { get; set; } = null!;
    }
}