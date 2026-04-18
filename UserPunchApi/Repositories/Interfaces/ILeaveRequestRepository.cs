using UserPunchApi.Models;


namespace UserPunchApi.Repositories.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestAsync();
        Task<IEnumerable<LeaveRequest>> GetByUserIdAsync(long userId);
        Task<LeaveRequest?> GetLeaveRequestByIdAsync(long id);
        Task AddLeaveRequestAsync(LeaveRequest leaveRequest);
        Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<bool> SaveChangesAsync();
    }
}