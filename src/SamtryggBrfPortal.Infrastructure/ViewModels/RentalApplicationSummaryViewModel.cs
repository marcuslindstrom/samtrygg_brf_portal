using System;
using SamtryggBrfPortal.Core.Enums;

namespace SamtryggBrfPortal.Infrastructure.ViewModels
{
    /// <summary>
    /// Summary view model for rental applications
    /// </summary>
    public class RentalApplicationSummaryViewModel
    {
        /// <summary>
        /// The rental application ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The property ID
        /// </summary>
        public Guid PropertyId { get; set; }

        /// <summary>
        /// The property address
        /// </summary>
        public string PropertyAddress { get; set; }

        /// <summary>
        /// The applicant's name
        /// </summary>
        public string ApplicantName { get; set; }

        /// <summary>
        /// The applicant's email
        /// </summary>
        public string ApplicantEmail { get; set; }

        /// <summary>
        /// The application status
        /// </summary>
        public RentalStatus Status { get; set; }

        /// <summary>
        /// The application status as a string
        /// </summary>
        public string StatusText => Status.ToString();

        /// <summary>
        /// The date the application was submitted
        /// </summary>
        public DateTime SubmittedAt { get; set; }

        /// <summary>
        /// The date the application was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// The rental start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The rental end date
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Whether the application has a background check
        /// </summary>
        public bool HasBackgroundCheck { get; set; }

        /// <summary>
        /// The background check status
        /// </summary>
        public BackgroundCheckStatus? BackgroundCheckStatus { get; set; }

        /// <summary>
        /// The number of unread messages
        /// </summary>
        public int UnreadMessagesCount { get; set; }
    }
}
