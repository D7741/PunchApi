using UserPunchApi.Models;

namespace UserPunchApi.Repositories.Interfaces
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>> GetAllScheduleAsync();
        Task<Schedule?> GetScheduleByIdAsync(long scheduleId);
        Task<IEnumerable<Schedule>> GetScheduleByUserIdAsync(long userId);
        Task<Schedule> CreateScheduleAsync(Schedule schedule);
        Task<Schedule?> UpdateScheduleAsync(Schedule schedule);
        Task<bool> DeleteScheduleAsync(long scheduleId);
    }
}