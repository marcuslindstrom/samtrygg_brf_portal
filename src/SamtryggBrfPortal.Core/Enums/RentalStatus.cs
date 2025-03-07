namespace SamtryggBrfPortal.Core.Enums
{
    public enum RentalStatus
    {
        Draft = 0,
        Pending = 1,
        PendingApproval = 1,
        Approved = 2,
        ApprovedByBrf = 2,
        Rejected = 3,
        RejectedByBrf = 3,
        PendingTenantBackgroundCheck = 4,
        PendingTenantApproval = 5,
        TenantApproved = 6,
        TenantRejected = 7,
        ContractSigned = 8,
        Active = 9,
        Completed = 10,
        Cancelled = 11
    }
}
