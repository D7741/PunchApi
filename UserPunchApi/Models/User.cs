using UserPunchApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserPunchApi.Models
{
    public enum UserType {Manager, Employee}

    public class User{

        [Key]
        [Required]
        public long UserId {get; set;}

        [Required]    
        public string UserName {get; set;} = string.Empty;

        [Required]
        public string Email {get; set;} = string.Empty;

        [Required]
        public string Password {get; set;} = string.Empty;

        public UserType Role {get; set;} = UserType.Employee;

        public int DepartmentId {get; set;}

        public Department? Department {get; set;}

        public List<Schedule> Schedules {get; set;} = new();

        public List<PunchRecord> PunchRecords {get; set;} = new();
        
        public List<LeaveRequest> LeaveRequests {get; set;} = new();
 
    }


}