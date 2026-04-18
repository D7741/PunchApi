using Microsoft.EntityFrameworkCore;
using UserPunchApi.Data;
using UserPunchApi.Models;
using UserPunchApi.Repositories.Interfaces;

namespace UserPunchApi.Repositories.Implementations
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly AppDbContext _context;

        public LeaveRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestAsync()
        {
            return await _context.LeaveRequests
                .Include(l => l.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetByUserIdAsync(long userId)
        {
            return await _context.LeaveRequests
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }

        public async Task<LeaveRequest?> GetLeaveRequestByIdAsync(long id)
        {
            return await _context.LeaveRequests
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.LeaveRequestId == id);
        }

        public async Task AddLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            await _context.LeaveRequests.AddAsync(leaveRequest);
        }

        public Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Update(leaveRequest);
            return Task.CompletedTask;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}