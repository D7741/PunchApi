using Microsoft.EntityFrameworkCore;
using UserPunchApi.Data;
using UserPunchApi.Models;
using UserPunchApi.Repositories.Interfaces;

namespace UserPunchApi.Repositories.Implementations
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAllDepartmentAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(long id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department?> UpdateDepartmentAsync(Department department)
        {
            var existing = await _context.Departments.FindAsync(department.DepartmentId);
            if (existing == null) return null;

            existing.DepartmentName = department.DepartmentName;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteDepartmentAsync(long id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return false;

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}