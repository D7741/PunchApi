using System.Collections;
using UserPunchApi.Models;

namespace UserPunchApi.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<IEnumerable<Schedule>> GetAllSchedulesAsync();
        Task<Schedule?> GetScheduleByIdAsync(long scheduleId);
        Task<IEnumerable<Schedule>> GetSchedulesByUserIdAsync(long userId);
        Task<Schedule> CreateScheduleAsync(Schedule schedule);
        Task<Schedule?> UpdateScheduleAsync(long scheduleId, Schedule schedule);
        Task<bool> DeleteScheduleAsync(long scheduleId);
    }
}