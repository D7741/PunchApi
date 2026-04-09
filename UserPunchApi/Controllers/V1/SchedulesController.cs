using Microsoft.AspNetCore.Mvc;
using UserPunchApi.Dtos.V1.SchedulesDtos;
using UserPunchApi.Models;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Controllers.V1
{
    [ApiController]
    [Route("api/v1/schedules")]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSchedules()
        {
            var schedules = await _scheduleService.GetAllSchedulesAsync();

            var response = schedules.Select(s => new ScheduleResponseDto
            {
                ScheduleId = s.ScheduleId,
                UserId = s.UserId,
                ShiftDate = s.ShiftDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                ShiftName = s.ShiftName,
                CreatedByManagerId = s.CreatedByManagerId
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleById(long id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);

            if (schedule == null)
            {
                return NotFound(new { message = $"Schedule with ID {id} not found." });
            }

            var response = new ScheduleResponseDto
            {
                ScheduleId = schedule.ScheduleId,
                UserId = schedule.UserId,
                ShiftDate = schedule.ShiftDate,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                ShiftName = schedule.ShiftName,
                CreatedByManagerId = schedule.CreatedByManagerId
            };

            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetSchedulesByUserId(long userId)
        {
            var schedules = await _scheduleService.GetSchedulesByUserIdAsync(userId);

            var response = schedules.Select(s => new ScheduleResponseDto
            {
                ScheduleId = s.ScheduleId,
                UserId = s.UserId,
                ShiftDate = s.ShiftDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                ShiftName = s.ShiftName,
                CreatedByManagerId = s.CreatedByManagerId
            });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var schedule = new Schedule
            {
                UserId = dto.UserId,
                ShiftDate = dto.ShiftDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                ShiftName = dto.ShiftName,
                CreatedByManagerId = dto.CreatedByManagerId
            };

            try
            {
                var createdSchedule = await _scheduleService.CreateScheduleAsync(schedule);

                var response = new ScheduleResponseDto
                {
                    ScheduleId = createdSchedule.ScheduleId,
                    UserId = createdSchedule.UserId,
                    ShiftDate = createdSchedule.ShiftDate,
                    StartTime = createdSchedule.StartTime,
                    EndTime = createdSchedule.EndTime,
                    ShiftName = createdSchedule.ShiftName,
                    CreatedByManagerId = createdSchedule.CreatedByManagerId
                };

                return CreatedAtAction(nameof(GetScheduleById), new { id = createdSchedule.ScheduleId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(long id, [FromBody] UpdateScheduleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var schedule = new Schedule
            {
                UserId = dto.UserId,
                ShiftDate = dto.ShiftDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                ShiftName = dto.ShiftName,
                CreatedByManagerId = dto.CreatedByManagerId
            };

            try
            {
                var updatedSchedule = await _scheduleService.UpdateScheduleAsync(id, schedule);

                if (updatedSchedule == null)
                {
                    return NotFound(new { message = $"Schedule with ID {id} not found." });
                }

                var response = new ScheduleResponseDto
                {
                    ScheduleId = updatedSchedule.ScheduleId,
                    UserId = updatedSchedule.UserId,
                    ShiftDate = updatedSchedule.ShiftDate,
                    StartTime = updatedSchedule.StartTime,
                    EndTime = updatedSchedule.EndTime,
                    ShiftName = updatedSchedule.ShiftName,
                    CreatedByManagerId = updatedSchedule.CreatedByManagerId
                };

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(long id)
        {
            var deleted = await _scheduleService.DeleteScheduleAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = $"Schedule with ID {id} not found." });
            }

            return Ok(new { message = $"Schedule with ID {id} deleted successfully." });
        }
    }
}