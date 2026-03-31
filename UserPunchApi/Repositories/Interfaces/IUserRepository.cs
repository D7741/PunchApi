namespace UserPunchApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(long id);

        Task<User?> GetByEmailAsync(string email);

        Task<bool> EmailExistsAsync(string email);

        Task<User> CreateAsync(User user);

        Task SaveChangesAsync();
    }
    
}