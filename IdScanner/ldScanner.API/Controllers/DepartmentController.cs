    using IdScanner.Application.Interface;
using IdScanner.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace ldScanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet("get-all-departments")]
        public async Task<IActionResult> GetAllUsers()
        {
            var departments = await _departmentService.GetAllDepartmentAsync();
            return Ok(departments);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateDepartment([FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _departmentService.CreateDepartmentAsync(department);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"This Department is not registered.");
            }

        }
        [HttpGet("GetById")]
        public IActionResult UserGetById(int departmentId)
        {
            try
            {
                var result = _departmentService.GetDepartmentDetailsById(departmentId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Department Not Found");
            }

        }
        [HttpPut("Update-User/{departmentId}")]
        public async Task<IActionResult> UpdateUserAsync(int departmentId, [FromBody] Department department)
        {

            var existingDepartment = _departmentService.GetDepartmentDetailsById(departmentId);
            if (existingDepartment == null && departmentId != department.DepartmentId)
            {
                return BadRequest("Department ID mismatch.");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    var updatedDepartment = await _departmentService.UpdateDepartmentAsync(departmentId, department);
                    return Ok(updatedDepartment);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = ex.Message });
                }
            }
        }
        [HttpDelete("Delete-department")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _departmentService.DeleteDepaetmentById(id);
                return Ok($"Department with ID {id} has been deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"Department with ID {id} not found: {ex.Message}");
            }
        }
    }
}
