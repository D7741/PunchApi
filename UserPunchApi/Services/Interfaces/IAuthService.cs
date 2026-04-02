using UserPunchApi.Models;
using UserPunchApi.Dtos.V1.AuthDtos;

namespace UserPunchApi.Services.Interfaces
{
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto?> RefreshTokenAsync(RefreshTokenDto dto);
    Task<User?> GetByEmailAsync(string email);
 
}