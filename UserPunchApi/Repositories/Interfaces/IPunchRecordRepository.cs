using UserPunchApi.Models;

namespace UserPunchApi.Repositories.Interfaces
{
    public interface IPunchRecordRepository
    {
        Task<IEnumerable<PunchRecord>> GetAllPunchRecordAsync();
        Task<PunchRecord?> GetPunchRecordByIdAsync(long id);
        Task<IEnumerable<PunchRecord>> GetPunchRecordByUserIdAsync(long userId);
        Task<PunchRecord?> GetOpenPunchRecordByUserIdAsync(long userId);
        Task<PunchRecord> CreateAsync(PunchRecord punchRecord);
        Task SaveChangesAsync();


    }
}