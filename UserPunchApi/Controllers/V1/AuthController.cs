using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPunchApi.Models.DTOs.Auth;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Controllers.V1
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            //await Task.CompletedTask;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 链接数据库，用service + repo来筛选
            var result = await _authService.LoginAsync(dto);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
        {
            //await Task.CompletedTask;

            if (string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                return BadRequest(new
                {
                    message = "Refresh token is required."
                });
            }

            // 这里校验 refresh token
            var result = await _authService.RefreshTokenAsync(dto);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid refresh token."
                });
            }

            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await Task.CompletedTask;

            // 以后如果你做 refresh token 存库
            // 这里就把数据库里的 refresh token 作废

            return Ok(new
            {
                message = "Logged out successfully."
            });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var email = User.FindFirst("email")?.Value;

            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized(new
                {
                    message = "User not found."
                });
            }

            var user = await _authService.GetByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                id = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                fullName = $"{user.FirstName} {user.LastName}",
                email = user.Email,
                role = user.Role
            });
        }
    }
}