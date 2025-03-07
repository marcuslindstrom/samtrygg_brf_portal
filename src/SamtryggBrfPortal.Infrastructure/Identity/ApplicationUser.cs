using Microsoft.AspNetCore.Identity;

namespace SamtryggBrfPortal.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PersonalNumber { get; set; } // Swedish personal number
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool HasCompletedOnboarding { get; set; }
        
        // For Swedish BankID integration
        public string? BankIdSubject { get; set; }
    }
}