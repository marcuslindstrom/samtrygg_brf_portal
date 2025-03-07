namespace SamtryggBrfPortal.Core.Entities
{
    public class BackgroundCheckDocument : BaseEntity
    {
        public Guid BackgroundCheckId { get; set; }
        public BackgroundCheck BackgroundCheck { get; set; } = null!;
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string DocumentType { get; set; } = string.Empty;
    }
}