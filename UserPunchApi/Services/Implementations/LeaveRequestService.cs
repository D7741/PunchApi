using UserPunchApi.Dtos.V1.LeaveRequestsDtos;
using UserPunchApi.Models;
using UserPunchApi.Repositories.Interfaces;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Services.Implementations
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<IEnumerable<LeaveRequestResponseDto>> GetAllLeaveRequestAsync()
        {
            var leaveRequests = await _leaveRequestRepository.GetAllLeaveRequestAsync();

            return leaveRequests.Select(l => new LeaveRequestResponseDto
            {
                LeaveRequestId = l.LeaveRequestId,
                UserId = l.UserId,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                LeaveReason = l.LeaveReason.ToString(),
                Status = l.Status
            });
        }

        public async Task<LeaveRequestResponseDto?> GetLeaveRequestByIdAsync(long id)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(id);

            if (leaveRequest == null)
                return null;

            return new LeaveRequestResponseDto
            {
                LeaveRequestId = leaveRequest.LeaveRequestId,
                UserId = leaveRequest.UserId,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                LeaveReason = leaveRequest.LeaveReason.ToString(),
                Status = leaveRequest.Status
            };
        }

        public async Task<LeaveRequestResponseDto> CreateLeaveRequestAsync(CreateLeaveRequestDto dto)
        {
            var leaveRequest = new LeaveRequest
            {
                UserId = dto.UserId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                LeaveReason = dto.LeaveReason,
                Status = "Pending"
            };

            await _leaveRequestRepository.AddLeaveRequestAsync(leaveRequest);
            await _leaveRequestRepository.SaveChangesAsync();

            return new LeaveRequestResponseDto
            {
                LeaveRequestId = leaveRequest.LeaveRequestId,
                UserId = leaveRequest.UserId,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                LeaveReason = leaveRequest.LeaveReason.ToString(),
                Status = leaveRequest.Status
            };
        }

        public async Task<bool> ApproveLeaveRequestAsync(long id)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(id);

            if (leaveRequest == null)
                return false;

            leaveRequest.Status = "Approved";

            await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);
            return await _leaveRequestRepository.SaveChangesAsync();
        }

        public async Task<bool> RejectLeaveRequestAsync(long id)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(id);

            if (leaveRequest == null)
                return false;

            leaveRequest.Status = "Rejected";

            await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);
            return await _leaveRequestRepository.SaveChangesAsync();
        }
    }
}