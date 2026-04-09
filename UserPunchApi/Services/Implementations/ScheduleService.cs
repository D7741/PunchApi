using UserPunchApi.Models;
using UserPunchApi.Repositories.Interfaces;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Services.Implementations
{
    public class ScheduleService: IScheduleService
    {
        public readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedulesAsync()
        {
            return await _scheduleRepository.GetAllScheduleAsync();
        }

        public async Task<Schedule?> GetScheduleByIdAsync(long scheduleId)
        {
            return await _scheduleRepository.GetScheduleByIdAsync(scheduleId);
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByUserIdAsync(long userId)
        {
            return await _scheduleRepository.GetScheduleByUserIdAsync(userId);
        }

        public async Task<Schedule> CreateScheduleAsync(Schedule schedule)
        {
            if (schedule.EndTime <= schedule.StartTime)
            {
                throw new ArgumentException("EndTime must be later than StartTime.");
            }

            return await _scheduleRepository.CreateScheduleAsync(schedule);
        }

        public async Task<Schedule?> UpdateScheduleAsync(long scheduleId, Schedule schedule)
        {
            if (schedule.EndTime <= schedule.StartTime)
            {
                throw new ArgumentException("EndTime must be later than StartTime.");
            }
            schedule.ScheduleId = scheduleId;
            return await _scheduleRepository.UpdateScheduleAsync(schedule);

        }

        public async Task<bool> DeleteScheduleAsync(long scheduleId)
        {
            return await _scheduleRepository.DeleteScheduleAsync(scheduleId);
        }
    }

}