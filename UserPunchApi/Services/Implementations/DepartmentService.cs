using UserPunchApi.Models;
using UserPunchApi.Repositories.Interfaces;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _departmentRepository.GetAllDepartmentAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(long id)
        {
            return await _departmentRepository.GetDepartmentByIdAsync(id);
        }

        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            return await _departmentRepository.CreateDepartmentAsync(department);
        }

        public async Task<Department?> UpdateDepartmentAsync(long id, Department department)
        {
            department.DepartmentId = id;
            return await _departmentRepository.UpdateDepartmentAsync(department);
        }

        public async Task<bool> DeleteDepartmentAsync(long id)
        {
            return await _departmentRepository.DeleteDepartmentAsync(id);
        }
    }
}