namespace SamtryggBrfPortal.Core.Entities
{
    public class Property : BaseEntity
    {
        public string Address { get; set; } = string.Empty;
        public string ApartmentNumber { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int FloorNumber { get; set; }
        public decimal Area { get; set; }
        public string Size => $"{Area} m²";
        public string Floor => FloorNumber.ToString();
        public int NumberOfRooms { get; set; }
        public string? Description { get; set; }
        public decimal MonthlyRent { get; set; }
        public bool IsAvailableForRent { get; set; }
        public Guid BrfAssociationId { get; set; }
        public BrfAssociation BrfAssociation { get; set; } = null!;
        public string OwnerId { get; set; } = string.Empty;
        public ICollection<RentalApplication> RentalApplications { get; set; } = new List<RentalApplication>();
        public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
        public string? PrimaryImageUrl { get; set; }
    }
}
