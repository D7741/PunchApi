using UserPunchApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserPunchApi.Models
{
    public class PunchRecord
    {
        [Key]
        [Required]
        public long PunchRecordId { get; set; }

        [Required]
        public long UserId { get; set; }  

        [ForeignKey("UserId")]
        public User? User { get; set; }
        
        [Required]
        public DateTime PunchInTime { get; set; }

        public DateTime? PunchOutTime { get; set; }

        public long? ModifiedByManagerId { get; set; }

        public User? ModifiedByManager { get; set; }
        
        public string? ModificationReason { get; set; }

    }
}