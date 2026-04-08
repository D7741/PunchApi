using UserPunchApi.Models;

namespace UserPunchApi.Services.Interfaces
{
    public interface IPunchRecordService
    {
        Task<List<PunchRecord>> GetAllPunchRecordAsync();
        Task<PunchRecord?> GetPunchRecordByIdAsync(long id);
        Task<List<PunchRecord>> GetPunchRecordByUserIdAsync(long userId);
        Task<(bool Success, string Message, PunchRecord? Record)> PunchInAsync(long userId);
        Task<(bool Success, string Message, PunchRecord? Record)> PunchOutAsync(long userId); 
    }
}