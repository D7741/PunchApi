using UserPunchApi.Models;

namespace UserPunchApi.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllDepartmentAsync();
        Task<Department?> GetDepartmentByIdAsync(long id);
        Task<Department> CreateDepartmentAsync(Department department);
        Task<Department?> UpdateDepartmentAsync(Department department);
        Task<bool> DeleteDepartmentAsync(long id);
    }
}