using Microsoft.AspNetCore.Mvc;
using UserPunchApi.Dtos.V1.DepartmentDtos;
using UserPunchApi.Models;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Controllers.V1
{
    [ApiController]
    [Route("api/v1/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();

            var response = departments.Select(d => new DepartmentResponseDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(long id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null) return NotFound();

            var response = new DepartmentResponseDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentDto dto)
        {
            var department = new Department
            {
                DepartmentName = dto.DepartmentName
            };

            var created = await _departmentService.CreateDepartmentAsync(department);

            var response = new DepartmentResponseDto
            {
                DepartmentId = created.DepartmentId,
                DepartmentName = created.DepartmentName
            };

            return CreatedAtAction(nameof(GetDepartmentById), new { id = created.DepartmentId }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(long id, CreateDepartmentDto dto)
        {
            var department = new Department
            {
                DepartmentName = dto.DepartmentName
            };

            var updated = await _departmentService.UpdateDepartmentAsync(id, department);
            if (updated == null) return NotFound();

            return Ok(new DepartmentResponseDto
            {
                DepartmentId = updated.DepartmentId,
                DepartmentName = updated.DepartmentName
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(long id)
        {
            var deleted = await _departmentService.DeleteDepartmentAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}