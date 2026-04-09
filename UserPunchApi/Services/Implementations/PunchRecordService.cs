using UserPunchApi.Common;
using UserPunchApi.Dtos.V1.PunchRecordsDtos;
using UserPunchApi.Models;
using UserPunchApi.Repositories.Interfaces;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Services.Implementations
{
    public class PunchRecordService : IPunchRecordService
    {
        private readonly IPunchRecordRepository _punchRecordRepository;
        private readonly IAuthRepository _authRepository;

        public PunchRecordService(
            IPunchRecordRepository punchRecordRepository,
            IAuthRepository authRepository)
        {
            _punchRecordRepository = punchRecordRepository;
            _authRepository = authRepository;
        }

        public async Task<IEnumerable<PunchRecord>> GetAllPunchRecordAsync()
        {
            return await _punchRecordRepository.GetAllPunchRecordAsync();
        }

        public async Task<PunchRecord?> GetPunchRecordByIdAsync(long punchRecordId)
        {
            return await _punchRecordRepository.GetPunchRecordByIdAsync(punchRecordId);
        }

        public async Task<IEnumerable<PunchRecord>> GetPunchRecordByUserIdAsync(long userId)
        {
            return await _punchRecordRepository.GetPunchRecordByUserIdAsync(userId);
        }

        public async Task<ServiceResult<PunchRecordResponseDto>> PunchInAsync(long userId)
        {
            var user = await _authRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ServiceResult<PunchRecordResponseDto>.Fail("User not found.");
            }

            var openRecord = await _punchRecordRepository.GetOpenPunchRecordByUserIdAsync(userId);
            if (openRecord != null)
            {
                return ServiceResult<PunchRecordResponseDto>.Fail("User already has an open punch record.");
            }

            var record = new PunchRecord
            {
                UserId = userId,
                PunchInTime = DateTime.UtcNow,
                PunchOutTime = null
            };

            await _punchRecordRepository.CreatePunchRecordAsync(record);

            var response = new PunchRecordResponseDto
            {
                PunchRecordId = record.PunchRecordId,
                UserId = record.UserId,
                PunchInTime = record.PunchInTime,
                PunchOutTime = record.PunchOutTime,
                Status = "Open"
            };

            return ServiceResult<PunchRecordResponseDto>.Ok(response, "Punch in successful.");
        }

        public async Task<ServiceResult<PunchRecordResponseDto>> PunchOutAsync(long userId)
        {
            var openRecord = await _punchRecordRepository.GetOpenPunchRecordByUserIdAsync(userId);
            if (openRecord == null)
            {
                return ServiceResult<PunchRecordResponseDto>.Fail("No open punch record found.");
            }

            openRecord.PunchOutTime = DateTime.UtcNow;

            await _punchRecordRepository.UpdatePunchRecordAsync(openRecord);

            var response = new PunchRecordResponseDto
            {
                PunchRecordId = openRecord.PunchRecordId,
                UserId = openRecord.UserId,
                PunchInTime = openRecord.PunchInTime,
                PunchOutTime = openRecord.PunchOutTime,
                Status = "Closed"
            };

            return ServiceResult<PunchRecordResponseDto>.Ok(response, "Punch out successful.");
        }
    }
}