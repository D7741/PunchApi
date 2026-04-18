using UserPunchApi.Models;
using UserPunchApi.Dtos.V1.LeaveRequestsDtos;

namespace UserPunchApi.Services.Interfaces
{
    public interface ILeaveRequestService
{
    Task<IEnumerable<LeaveRequestResponseDto>> GetAllLeaveRequestAsync();
    Task<IEnumerable<LeaveRequestResponseDto>> GetMyLeaveRequestsAsync(long userId);
    Task<LeaveRequestResponseDto?> GetLeaveRequestByIdAsync(long id);
    Task<LeaveRequestResponseDto> CreateLeaveRequestAsync(CreateLeaveRequestDto dto);
    Task<bool> ApproveLeaveRequestAsync(long id);
    Task<bool> RejectLeaveRequestAsync(long id);
}
}