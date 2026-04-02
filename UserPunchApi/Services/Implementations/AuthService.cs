namespace Service.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

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
            var emailExists = await _userRepository.EmailExistsAsync(dto.Email);

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

            var createdUser = await _userRepository.CreateAsync(user);

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

            // 这里先做占位逻辑
            // 真正版本会去数据库验证 refresh token
            if (!dto.RefreshToken.StartsWith("refresh-token-"))
            {
                return null;
            }

            var fakeUser = await _userRepository.GetByEmailAsync("manager@test.com");

            if (fakeUser == null)
            {
                return null;
            }

            return new AuthResponseDto
            {
                UserId = fakeUser.Id,
                FullName = $"{fakeUser.FirstName} {fakeUser.LastName}",
                Email = fakeUser.Email,
                Role = fakeUser.Role,
                AccessToken = GenerateFakeAccessToken(fakeUser),
                RefreshToken = GenerateFakeRefreshToken(fakeUser)
            };
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
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