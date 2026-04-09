using UserPunchApi.Models;

namespace UserPunchApi.Repositories.Interfaces
{
    public interface IPunchRecordRepository
    {
        Task<IEnumerable<PunchRecord>> GetAllPunchRecordAsync();
        Task<PunchRecord?> GetPunchRecordByIdAsync(long punchRecordId);
        Task<IEnumerable<PunchRecord>> GetPunchRecordByUserIdAsync(long userId);
        Task<PunchRecord?> GetOpenPunchRecordByUserIdAsync(long userId);
        Task<PunchRecord> CreatePunchRecordAsync(PunchRecord punchRecord);
        Task<PunchRecord?> UpdatePunchRecordAsync(PunchRecord punchRecord);
        Task SaveChangesAsync();


    }
}