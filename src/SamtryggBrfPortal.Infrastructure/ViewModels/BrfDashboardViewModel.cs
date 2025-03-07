using System;
using System.Collections.Generic;

namespace SamtryggBrfPortal.Infrastructure.ViewModels
{
    /// <summary>
    /// View model for the BRF dashboard
    /// </summary>
    public class BrfDashboardViewModel
    {
        /// <summary>
        /// The BRF association ID
        /// </summary>
        public Guid BrfId { get; set; }

        /// <summary>
        /// The BRF association name
        /// </summary>
        public string BrfName { get; set; }

        /// <summary>
        /// The number of properties in the BRF association
        /// </summary>
        public int TotalProperties { get; set; }

        /// <summary>
        /// The number of available properties
        /// </summary>
        public int AvailableProperties { get; set; }

        /// <summary>
        /// The number of pending rental applications
        /// </summary>
        public int PendingApplications { get; set; }

        /// <summary>
        /// The number of approved rental applications
        /// </summary>
        public int ApprovedApplicationsCount { get; set; }

        /// <summary>
        /// The number of rejected rental applications
        /// </summary>
        public int RejectedApplicationsCount { get; set; }

        /// <summary>
        /// The number of board members
        /// </summary>
        public int BoardMemberCount { get; set; }

        /// <summary>
        /// Recent rental applications
        /// </summary>
        public List<RentalApplicationSummaryViewModel> RecentApplications { get; set; } = new List<RentalApplicationSummaryViewModel>();

        /// <summary>
        /// Available properties
        /// </summary>
        public List<PropertySummaryViewModel> AvailablePropertiesList { get; set; } = new List<PropertySummaryViewModel>();
    }
}
