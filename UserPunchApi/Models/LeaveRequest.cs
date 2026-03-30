using UserPunchApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserPunchApi.Models
{
    public enum Reson
    {
        Annual_Leave,
        Alt_Holiday_Leave,
        Personal_Sick,
        Person_Carers,
        Long_Service_Leave
      
    }
    public class LeaveRequest
    {
        [Key]
        public long LeaveRequestId {get; set;}

        public long UserId {get; set;}

        [ForeignKey("UserId")]
        public User? User {get; set;}

        public DateTime StartDate {get; set;}

        public DateTime EndDate {get; set;}

        public Reson LeaveReason {get; set;} = Reson.Person_Carers;

        public string Status {get; set;} = "Peding";

    }
}