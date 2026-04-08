using UserPunchApi.Models;
using UserPunchApi.Repositories.Interfaces;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Services.Implementations
{
    public class PunchRecordService: IPunchRecordService
    {
        public readonly IPunchRecordRepository _punchRecordRepository;

        public PunchRecordService(IPunchRecordRepository punchRecordRepository)
        {
            _punchRecordRepository = punchRecordRepository;
        }

        public async Task<List<PunchRecord>> GetAllPunchRecordAsync()
        {
            return await _punchRecordRepository.GetAllPunchRecordAsync();
        }

        public async Task<PunchRecord?> GetPunchRecordByIdAsync(long id)
        {
            return await _punchRecordRepository.GetPunchRecordByIdAsync(id);
        }

        public async Task<List<PunchRecord>> GetPunchRecordByUserIdAsync(long userId)
        {
            return await _punchRecordRepository.GetPunchRecordByUserIdAsync(userId);
        }

        public async Task<(bool Success, string Message, PunchRecord? Record)> PunchInAsync(long userId)
        {
            var openRecord = await _punchRecordRepository.GetOpenPunchRecordByUserIdAsync(userId);

            if (openRecord != null)
            {
                return (false, "User already punched in and has not punched out yet.", null);
            }

            var newRecord = new PunchRecord
            {
                UserId = userId,
                PunchInTime = DateTime.UtcNow,
                PunchOutTime = null
            };

            var createdRecord = await _punchRecordRepository.CreateAsync(newRecord);

            return (true, "Punch in successful.", createdRecord);
        }

        public async Task<(bool Success, string Message, PunchRecord? Record)> PunchOutAsync(long userId)
        {
            var openRecord = await _punchRecordRepository.GetOpenPunchRecordByUserIdAsync(userId);

            if (openRecord == null)
            {
                return (false, "No active punch-in record found.", null);
            }

            openRecord.PunchOutTime = DateTime.UtcNow;
            await _punchRecordRepository.SaveChangesAsync();

            return (true, "Punch out successful.", openRecord);
        }
    }
}