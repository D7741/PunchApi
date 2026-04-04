using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserPunchApi.Controllers.V1
{
    [Route("api/v1/punch-records")]
    [ApiController]
    //[Authorize]
    public class PunchRecordsController : ControllerBase
    {
        public PunchRecordsController()
        {
        }

        [HttpPost("punch-in")]
        public async Task<IActionResult> PunchIn([FromBody] object dto)
        {
            return Ok("Punch in success");
        }

        [HttpPost("punch-out")]
        public async Task<IActionResult> PunchOut([FromBody] object dto)
        {
            return Ok("Punch out success");
        }

        [HttpPost]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreatePunchRecord([FromBody] object dto)
        {
            return Ok("Create punch record");
        }

        [HttpPatch("{id}")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdatePunchRecord(int id, [FromBody] object dto)
        {
            return Ok($"Update punch record {id}");
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeletePunchRecord(int id)
        {
            return Ok($"Delete punch record {id}");
        }
    }
}