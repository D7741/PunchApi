using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPunchApi.Common;
using UserPunchApi.Dtos.V1.DepartmentDtos;
using UserPunchApi.Models;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Controllers.V1
{
    // Departments are an admin concern — only managers can manage them.
    [ApiController]
    [Route("api/v1/departments")]
    [Authorize(Roles = Roles.Manager)]
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

            return Ok(new DepartmentResponseDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto dto)
        {
            var department = new Department { DepartmentName = dto.DepartmentName };
            var created = await _departmentService.CreateDepartmentAsync(department);

            return CreatedAtAction(nameof(GetDepartmentById), new { id = created.DepartmentId },
                new DepartmentResponseDto
                {
                    DepartmentId = created.DepartmentId,
                    DepartmentName = created.DepartmentName
                });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(long id, [FromBody] CreateDepartmentDto dto)
        {
            var department = new Department { DepartmentName = dto.DepartmentName };
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
