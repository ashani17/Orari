using Microsoft.AspNetCore.Mvc;
using Orari.DTO.DepratmentDTO;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/department")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartments();
            return Ok(departments);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentByIdAsync([FromBody] GetDelDepartmentDTO dto)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(dto.DId);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDepartmentAsync([FromBody] PutDepartmentDTO department)
        {
            if (department == null)
            {
                return BadRequest();
            }
            // Map the DTO to the Departments model
            var departmentModel = new Departments
            {
                DName = department.DName,
            };
            var createdDepartment = await _departmentService.CreateDepartmentAsync(departmentModel);
            return CreatedAtAction(nameof(GetDepartmentByIdAsync), new { id = createdDepartment.DId }, createdDepartment);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartmentAsync(int id, [FromBody] PutDepartmentDTO department)
        {
            if (department == null)
            {
                return BadRequest();
            }

            // Map the DTO to the Departments model
            var departmentModel = new Departments
            {
                DName = department.DName
            };

            var updatedDepartment = await _departmentService.UpdateDepartmentAsync(departmentModel);
            if (updatedDepartment == null)
            {
                return NotFound();
            }
            return Ok(updatedDepartment);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] GetDelDepartmentDTO dto)
        {
            var deleted = await _departmentService.DeleteDepartmentAsync(dto.DId);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
