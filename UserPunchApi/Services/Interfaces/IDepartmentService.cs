using UserPunchApi.Models;

namespace UserPunchApi.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllDepartmentsAsync();
        Task<Department?> GetDepartmentByIdAsync(long id);
        Task<Department> CreateDepartmentAsync(Department department);
        Task<Department?> UpdateDepartmentAsync(long id, Department department);
        Task<bool> DeleteDepartmentAsync(long id);
    }
}