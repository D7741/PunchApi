using Microsoft.AspNetCore.Mvc;
using UserPunchApi.DTOs.V1.PunchRecordsDtos;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Controllers.V1
{
    [ApiController]
    [Route("api/v1/punchrecords")]
    public class PunchRecordsController : ControllerBase
    {
        private readonly IPunchRecordService _punchRecordService;

        public PunchRecordsController(IPunchRecordService punchRecordService)
        {
            _punchRecordService = punchRecordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPunchRecord()
        {
            var records = await _punchRecordService.GetAllPunchRecordAsync();

            var response = records.Select(r => new PunchRecordResponseDto
            {
                PunchRecordId = r.PunchRecordId,
                UserId = r.UserId,
                PunchInTime = r.PunchInTime,
                PunchOutTime = r.PunchOutTime,
                Status = r.PunchOutTime == null ? "Open" : "Closed"
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPunchRecordById(long id)
        {
            var record = await _punchRecordService.GetPunchRecordByIdAsync(id);

            if (record == null)
            {
                return NotFound(new { message = "Punch record not found." });
            }

            var response = new PunchRecordResponseDto
            {
                PunchRecordId = record.PunchRecordId,
                UserId = record.UserId,
                PunchInTime = record.PunchInTime,
                PunchOutTime = record.PunchOutTime,
                Status = record.PunchOutTime == null ? "Open" : "Closed"
            };

            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPunchRecordByUserId(long userId)
        {
            var records = await _punchRecordService.GetPunchRecordByUserIdAsync(userId);

            var response = records.Select(r => new PunchRecordResponseDto
            {
                PunchRecordId = r.PunchRecordId,
                UserId = r.UserId,
                PunchInTime = r.PunchInTime,
                PunchOutTime = r.PunchOutTime,
                Status = r.PunchOutTime == null ? "Open" : "Closed"
            });

            return Ok(response);
        }

        [HttpPost("punchin")]
        public async Task<IActionResult> PunchIn([FromBody] PunchInRequestDto request)
        {
            var result = await _punchRecordService.PunchInAsync(request.UserId);
            var record = result.Record;

            if (!result.Success || record == null)
            {
                return BadRequest(new { message = result.Message });
            }

            var response = new PunchRecordResponseDto
            {
                PunchRecordId = record.PunchRecordId,
                UserId = record.UserId,
                PunchInTime = record.PunchInTime,
                PunchOutTime = record.PunchOutTime,
                Status = record.PunchOutTime == null ? "Open" : "Closed"
            };

            return Ok(new
            {
                message = result.Message,
                data = response
            });
        }

        [HttpPost("punchout")]
        public async Task<IActionResult> PunchOut([FromBody] PunchOutRequestDto request)
        {
            var result = await _punchRecordService.PunchOutAsync(request.UserId);
            var record = result.Record;

            if (!result.Success || record == null)
            {
                return BadRequest(new { message = result.Message });
            }

            var response = new PunchRecordResponseDto
            {
                PunchRecordId = record.PunchRecordId,
                UserId = record.UserId,
                PunchInTime = record.PunchInTime,
                PunchOutTime = record.PunchOutTime,
                Status = record.PunchOutTime == null ? "Open" : "Closed"
            };

            return Ok(new
            {
                message = result.Message,
                data = response
            });
        }
    }
}