using UserPunchApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace UserPunchApi.Models
{
    public class Schedule
    {
        [Key]
        public long ScheduleId {get; set;}
        public long UserId {get; set;}

        [ForeignKey("UserId")]
        public User? User {get; set;}

        public DateTime ShiftDate {get; set;}

        public DateTime StartTime {get; set;}

        public DateTime EndTime {get; set;}

        public long? CreatedByManagerId { get; set; }
        
        public User? CreatedByManager { get; set; }

    }
}
