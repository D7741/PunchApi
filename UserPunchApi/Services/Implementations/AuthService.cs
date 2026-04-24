using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using UserPunchApi.Common;
using UserPunchApi.Models;
using UserPunchApi.Dtos.V1.AuthDtos;
using UserPunchApi.Repositories.Interfaces;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IJwtTokenService jwtTokenService, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _authRepository.GetByEmailAsync(dto.Email);

            if (user == null || !user.IsActive)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            return BuildAuthResponse(user);
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var emailExists = await _authRepository.EmailExistsAsync(dto.Email);

            if (emailExists)
                throw new InvalidOperationException("This email is already registered.");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _authRepository.CreateAsync(user);
            return BuildAuthResponse(createdUser);
        }

        public async Task<AuthResponseDto?> RefreshTokenAsync(RefreshTokenDto dto)
        {
            // Dev note: right now we accept any non-empty refresh token and look up
            // the user by the userId embedded in the DTO. In production you would:
            //   1. Store the refresh token hash in the DB when it's issued
            //   2. Look it up here and verify it hasn't expired or been revoked
            //   3. Delete the old one and issue a new pair (token rotation)
            if (string.IsNullOrWhiteSpace(dto.RefreshToken))
                return null;

            var user = await _authRepository.GetByIdAsync(dto.UserId);

            if (user == null || !user.IsActive)
                return null;

            return BuildAuthResponse(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _authRepository.GetByEmailAsync(email);
        }

        public async Task<AuthResponseDto?> GoogleLoginAsync(string credential)
        {
            var clientId = _configuration["Authentication:Google:ClientId"];

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(credential,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { clientId }
                    });
            }
            catch
            {
                return null;
            }

            var user = await _authRepository.GetByEmailAsync(payload.Email);

            if (user == null)
            {
                var nameParts = (payload.Name ?? "").Split(' ', 2);
                user = new User
                {
                    FirstName = nameParts.Length > 0 ? nameParts[0] : payload.Email,
                    LastName = nameParts.Length > 1 ? nameParts[1] : "",
                    Email = payload.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                    Role = Roles.Employee,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                user = await _authRepository.CreateAsync(user);
            }
            else if (!user.IsActive)
            {
                return null;
            }

            return BuildAuthResponse(user);
        }

        // Private helper — avoids repeating the same DTO mapping in Login, Register, Refresh
        private AuthResponseDto BuildAuthResponse(User user)
        {
            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Role = user.Role,
                AccessToken = _jwtTokenService.GenerateAccessToken(user),
                RefreshToken = _jwtTokenService.GenerateRefreshToken()
            };
        }
    }
}
