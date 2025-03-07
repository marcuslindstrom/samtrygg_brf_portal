namespace SamtryggBrfPortal.Core.Entities
{
    public class BrfAssociation : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string OrganizationNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Property> Properties { get; set; } = new List<Property>();
        public ICollection<BrfBoardMember> BoardMembers { get; set; } = new List<BrfBoardMember>();
        public ICollection<BrfDocument> Documents { get; set; } = new List<BrfDocument>();
    }
}