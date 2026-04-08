using UserPunchApi.Models;

namespace UserPunchApi.Repositories.Interfaces
{
    public interface IPunchRecordRepository
    {
        Task<List<PunchRecord>> GetAllPunchRecordAsync();
        Task<PunchRecord?> GetPunchRecordByIdAsync(long id);
        Task<List<PunchRecord>> GetPunchRecordByUserIdAsync(long userId);
        Task<PunchRecord?> GetOpenPunchRecordByUserIdAsync(long userId);
        Task<PunchRecord> CreateAsync(PunchRecord punchRecord);
        Task SaveChangesAsync();


    }
}