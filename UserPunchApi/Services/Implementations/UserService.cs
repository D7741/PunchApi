using UserPunchApi.Models;
using UserPunchApi.Repositories.Interfaces;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByIdAsync(long id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null) return null;

            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<User?> UpdateUserAsync(long id, User user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null) return null;

            if (!existingUser.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailUsed = await _userRepository.GetUserByEmailAsync(user.Email);
                if (emailUsed != null) return null;
            }

            user.Id = id;
            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }
    }
}