using System.ComponentModel.DataAnnotations;

namespace UserPunchApi.Models
{
    public class User
    {
        public long Id { get; set; }

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";

        public string Role { get; set; } = "Employee";

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // build relation between entities.

        public long? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public ICollection<PunchRecord> PunchRecords { get; set; } = new List<PunchRecord>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}