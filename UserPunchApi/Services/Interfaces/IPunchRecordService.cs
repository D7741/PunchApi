using UserPunchApi.Common;
using UserPunchApi.Dtos.V1.PunchRecordsDtos;
using UserPunchApi.Models;

namespace UserPunchApi.Services.Interfaces
{
    public interface IPunchRecordService
    {
        Task<IEnumerable<PunchRecord>> GetAllPunchRecordAsync();
        Task<PunchRecord?> GetPunchRecordByIdAsync(long punchRecordId);
        Task<IEnumerable<PunchRecord>> GetPunchRecordByUserIdAsync(long userId);

        Task<ServiceResult<PunchRecordResponseDto>> PunchInAsync(long userId);
        Task<ServiceResult<PunchRecordResponseDto>> PunchOutAsync(long userId);
    }
}