
using System.ComponentModel.DataAnnotations;


namespace UserPunchApi.Dtos.V1.LeaveRequestsDtos
{
    public class LeaveRequestResponseDto
    {
        public long LeaveRequestId { get; set; }
        public long UserId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string LeaveReason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}

