namespace UserPunchApi.DTOs.V1.PunchRecordsDtos
{
    public class PunchRecordResponseDto
    {
        public long PunchRecordId { get; set; }
        public long UserId { get; set; }
        public DateTime PunchInTime { get; set; }
        public DateTime? PunchOutTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}