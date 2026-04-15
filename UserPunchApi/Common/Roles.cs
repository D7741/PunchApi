namespace UserPunchApi.Common
{
    // Same idea as PunchRecordStatus / LeaveRequestStatus.
    // Centralise role names so [Authorize(Roles = Roles.Manager)]
    // and [Authorize(Roles = Roles.Employee)] never drift apart.
    public static class Roles
    {
        public const string Manager = "Manager";
        public const string Employee = "Employee";
    }
}
