using UserPunchApi.Models;

namespace UserPunchApi.Services.Interfaces
{
    public interface IPunchRecordService
    {
        Task<IEnumerable<PunchRecord>> GetAllPunchRecordAsync();
        Task<PunchRecord?> GetPunchRecordByIdAsync(long id);
        Task<IEnumerable<PunchRecord>> GetPunchRecordByUserIdAsync(long userId);
        Task<(bool Success, string Message, PunchRecord? Record)> PunchInAsync(long userId);
        Task<(bool Success, string Message, PunchRecord? Record)> PunchOutAsync(long userId); 
    }
}