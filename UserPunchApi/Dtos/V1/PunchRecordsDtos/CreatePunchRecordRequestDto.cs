namespace UserPunchApi.Dtos.V1.PunchRecordsDtos
{
    public class CreatePunchRecordRequestDto
    {
        public long UserId { get; set; }
        public DateTime PunchInTime { get; set; }
        public DateTime? PunchOutTime { get; set; }
    }
}