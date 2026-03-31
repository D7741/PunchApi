using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPunchApi.Models.DTOs.Auth;

namespace UserPunchApi.Controllers.V1
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            await Task.CompletedTask;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 这里先写死，后面再接数据库
            if (dto.Email != "manager@test.com" || dto.Password != "123456")
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            var response = new AuthResponseDto
            {
                UserId = 1,
                FullName = "Test Manager",
                Email = dto.Email,
                Role = "Manager",
                AccessToken = "fake-jwt-access-token",
                RefreshToken = "fake-refresh-token"
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            await Task.CompletedTask;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 这里以后要查数据库看 email 是否已存在
            if (dto.Email == "manager@test.com")
            {
                return Conflict(new
                {
                    message = "This email is already registered."
                });
            }

            var response = new AuthResponseDto
            {
                UserId = 2,
                FullName = $"{dto.FirstName} {dto.LastName}",
                Email = dto.Email,
                Role = dto.Role,
                AccessToken = "fake-jwt-access-token-for-new-user",
                RefreshToken = "fake-refresh-token-for-new-user"
            };

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
        {
            await Task.CompletedTask;

            if (string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                return BadRequest(new
                {
                    message = "Refresh token is required."
                });
            }

            // 这里以后改成真正校验 refresh token
            if (dto.RefreshToken != "fake-refresh-token" &&
                dto.RefreshToken != "fake-refresh-token-for-new-user")
            {
                return Unauthorized(new
                {
                    message = "Invalid refresh token."
                });
            }

            return Ok(new
            {
                accessToken = "new-fake-jwt-access-token",
                refreshToken = "new-fake-refresh-token"
            });
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
            await Task.CompletedTask;

            var user = new
            {
                id = 1,
                fullName = User.Identity?.Name ?? "Current User",
                email = User.FindFirst("email")?.Value ?? "manager@test.com",
                role = User.FindFirst("role")?.Value ?? "Manager"
            };

            return Ok(user);
        }
    }
}