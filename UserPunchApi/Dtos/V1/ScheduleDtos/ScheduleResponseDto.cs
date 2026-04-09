namespace UserPunchApi.Dtos.V1.SchedulesDtos
{
    public class ScheduleResponseDto
    {
        public long ScheduleId { get; set; }
        public long UserId { get; set; }
        public DateTime ShiftDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ShiftName { get; set; } = string.Empty;
        public long? CreatedByManagerId { get; set; }
    }
}