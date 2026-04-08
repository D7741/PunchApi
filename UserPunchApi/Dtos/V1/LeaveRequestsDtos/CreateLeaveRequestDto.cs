using System.ComponentModel.DataAnnotations;
using UserPunchApi.Models;

namespace UserPunchApi.Dtos.V1.LeaveRequestsDtos
{
    public class CreateLeaveRequestDto
    {
        public long UserId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Reason LeaveReason { get; set; }
    }
}

