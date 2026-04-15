using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPunchApi.Common;
using UserPunchApi.Dtos.V1.UserDtos;
using UserPunchApi.Models;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Controllers.V1
{
    // Every endpoint in this controller requires a Manager role.
    // Only managers should be able to create, update, or delete user accounts.
    [ApiController]
    [Route("api/v1/users")]
    [Authorize(Roles = Roles.Manager)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            var response = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                DepartmentId = u.DepartmentId,
                DepartmentName = u.Department?.DepartmentName
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(long id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");

            var response = new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department?.DepartmentName
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = dto.Password,
                Role = dto.Role,
                IsActive = dto.IsActive,
                DepartmentId = dto.DepartmentId
            };

            var createdUser = await _userService.CreateUserAsync(user);
            if (createdUser == null)
                return BadRequest("Email already exists.");

            var response = new UserResponseDto
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
                Role = createdUser.Role,
                IsActive = createdUser.IsActive,
                CreatedAt = createdUser.CreatedAt,
                DepartmentId = createdUser.DepartmentId,
                DepartmentName = createdUser.Department?.DepartmentName
            };

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] UpdateUserDto dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Role = dto.Role,
                IsActive = dto.IsActive,
                DepartmentId = dto.DepartmentId
            };

            var updatedUser = await _userService.UpdateUserAsync(id, user);
            if (updatedUser == null)
                return NotFound("User not found or email already in use.");

            var response = new UserResponseDto
            {
                Id = updatedUser.Id,
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                Email = updatedUser.Email,
                Role = updatedUser.Role,
                IsActive = updatedUser.IsActive,
                CreatedAt = updatedUser.CreatedAt,
                DepartmentId = updatedUser.DepartmentId,
                DepartmentName = updatedUser.Department?.DepartmentName
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted) return NotFound("User not found.");

            return NoContent();
        }
    }
}
