namespace SamtryggBrfPortal.Core.Entities
{
    public class BrfBoardMember : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public Guid BrfAssociationId { get; set; }
        public BrfAssociation BrfAssociation { get; set; } = null!;
        public bool IsChairperson { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime MemberSince { get; set; }
        public DateTime? MemberUntil { get; set; }
    }
}