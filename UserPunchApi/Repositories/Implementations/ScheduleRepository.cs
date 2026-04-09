using Microsoft.EntityFrameworkCore;
using UserPunchApi.Data;
using UserPunchApi.Models;
using UserPunchApi.Repositories.Interfaces;

namespace UserPunchApi.Repositories.Implementations
{
    public class ScheduleRepository: IScheduleRepository
    {
        private readonly AppDbContext _context;

        public ScheduleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Schedule>> GetAllScheduleAsync()
        {
            return await _context.Schedules
                .Include(s => s.User)
                .Include(s => s.CreatedByManager)
                .ToListAsync();
        }

        public async Task<Schedule?> GetScheduleByIdAsync(long scheduleId)
        {
            return await _context.Schedules
                .Include(s => s.User)
                .Include(s => s.CreatedByManager)
                .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);
        }

        public async Task<IEnumerable<Schedule>> GetScheduleByUserIdAsync(long userId)
        {
            return await _context.Schedules
                .Where(s => s.UserId == userId)
                .Include(s => s.User)
                .Include(s => s.CreatedByManager)
                .ToListAsync();
        }

        public async Task<Schedule> CreateScheduleAsync(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<Schedule?> UpdateScheduleAsync(Schedule schedule)
        {
            var existingSchedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.ScheduleId == schedule.ScheduleId);

            if (existingSchedule == null)
            {
                return null;
            }

            existingSchedule.UserId = schedule.UserId;
            existingSchedule.ShiftDate = schedule.ShiftDate;
            existingSchedule.StartTime = schedule.StartTime;
            existingSchedule.EndTime = schedule.EndTime;
            existingSchedule.ShiftName = schedule.ShiftName;
            existingSchedule.CreatedByManagerId = schedule.CreatedByManagerId;

            await _context.SaveChangesAsync();
            return existingSchedule;        
        }

        public async Task<bool> DeleteScheduleAsync(long scheduleId)
        {
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);

            if (schedule == null)
            {
                return false;
            }

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }

    }
    
}