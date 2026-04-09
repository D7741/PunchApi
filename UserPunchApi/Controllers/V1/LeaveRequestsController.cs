using Microsoft.AspNetCore.Mvc;
using UserPunchApi.Dtos.V1.LeaveRequestsDtos;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Controllers.V1
{
    [ApiController]
    [Route("api/v1/leaverequests")]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestsController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLeaveRequest()
        {
            var requests = await _leaveRequestService.GetAllLeaveRequestAsync();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeaveRequestById(long id)
        {
            var request = await _leaveRequestService.GetLeaveRequestByIdAsync(id);

            if (request == null)
                return NotFound();

            return Ok(request);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequestDto dto)
        {
            var created = await _leaveRequestService.CreateLeaveRequestAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.LeaveRequestId }, created);
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveLeaveRequest(long id)
        {
            var result = await _leaveRequestService.ApproveLeaveRequestAsync(id);

            if (!result)
                return NotFound();

            return Ok("Approved");
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectLeaveRequest(long id)
        {
            var result = await _leaveRequestService.RejectLeaveRequestAsync(id);

            if (!result)
                return NotFound();

            return Ok("Rejected");
        }
    }
}