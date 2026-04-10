using UserPunchApi.Models;

namespace UserPunchApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(long id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> CreateUserAsync(User user);
        Task<User?> UpdateUserAsync(long id, User user);
        Task<bool> DeleteUserAsync(long id);
    }
}