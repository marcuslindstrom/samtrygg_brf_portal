namespace SamtryggBrfPortal.Core.Entities
{
    public class PropertyImage : BaseEntity
    {
        public string ImageUrl { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsPrimary { get; set; }
        public Guid PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }
}