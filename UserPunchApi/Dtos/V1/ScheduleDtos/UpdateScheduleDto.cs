using System.ComponentModel.DataAnnotations;

namespace UserPunchApi.Dtos.V1.SchedulesDtos
{
    public class UpdateScheduleDto
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public DateTime ShiftDate { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [StringLength(100)]
        public string ShiftName { get; set; } = string.Empty;

        public long? CreatedByManagerId { get; set; }
    }
}