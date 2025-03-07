using System;

namespace SamtryggBrfPortal.Infrastructure.ViewModels
{
    /// <summary>
    /// Summary view model for properties
    /// </summary>
    public class PropertySummaryViewModel
    {
        /// <summary>
        /// The property ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The property address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The property size in square meters
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// The number of rooms
        /// </summary>
        public int NumberOfRooms { get; set; }

        /// <summary>
        /// The floor number
        /// </summary>
        public string Floor { get; set; }

        /// <summary>
        /// The monthly rent
        /// </summary>
        public decimal MonthlyRent { get; set; }

        /// <summary>
        /// Whether the property is available for rent
        /// </summary>
        public bool IsAvailableForRent { get; set; }

        /// <summary>
        /// The date the property was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// The URL to the property's primary image
        /// </summary>
        public string PrimaryImageUrl { get; set; }

        /// <summary>
        /// The number of active rental applications for this property
        /// </summary>
        public int ActiveApplicationsCount { get; set; }
    }
}
