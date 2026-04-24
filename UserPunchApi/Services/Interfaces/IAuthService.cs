using UserPunchApi.Models;
using UserPunchApi.Dtos.V1.AuthDtos;

namespace UserPunchApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> RefreshTokenAsync(RefreshTokenDto dto);
        Task<User?> GetByEmailAsync(string email);
        Task<AuthResponseDto?> GoogleLoginAsync(string credential);
    }
 
}