using UserPunchApi.Models;
using UserPunchApi.Dtos.V1.AuthDtos;
using UserPunchApi.Repositories.Interfaces;
using UserPunchApi.Repositories.Implementations;
using UserPunchApi.Services.Interfaces;
using UserPunchApi.Services.Implementations;


namespace UserPunchApi.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _authRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                return null;
            }

            if (!user.IsActive)
            {
                return null;
            }

            // 现在先用明文对比，后面再换 PasswordHash
            if (user.Password != dto.Password)
            {
                return null;
            }

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Role = user.Role,
                AccessToken = GenerateFakeAccessToken(user),
                RefreshToken = GenerateFakeRefreshToken(user)
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var emailExists = await _authRepository.EmailExistsAsync(dto.Email);

            if (emailExists)
            {
                throw new InvalidOperationException("This email is already registered.");
            }

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _authRepository.CreateAsync(user);

            return new AuthResponseDto
            {
                UserId = createdUser.Id,
                FullName = $"{createdUser.FirstName} {createdUser.LastName}",
                Email = createdUser.Email,
                Role = createdUser.Role,
                AccessToken = GenerateFakeAccessToken(createdUser),
                RefreshToken = GenerateFakeRefreshToken(createdUser)
            };
        }

        public async Task<AuthResponseDto?> RefreshTokenAsync(RefreshTokenDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                return null;
            }

            if (!dto.RefreshToken.StartsWith("refresh-token-"))
            {
                return null;
            }

            var parts = dto.RefreshToken.Split('-');

            if (parts.Length < 3)
            {
                return null;
            }

            if (!long.TryParse(parts[2], out var userId))
            {
                return null;
            }

            var user = await _authRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Role = user.Role,
                AccessToken = GenerateFakeAccessToken(user),
                RefreshToken = GenerateFakeRefreshToken(user)
            };
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _authRepository.GetByEmailAsync(email);
        }

        private string GenerateFakeAccessToken(User user)
        {
            return $"access-token-{user.Id}-{Guid.NewGuid()}";
        }

        private string GenerateFakeRefreshToken(User user)
        {
            return $"refresh-token-{user.Id}-{Guid.NewGuid()}";
        }
    }
}