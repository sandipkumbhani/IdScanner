using Microsoft.AspNetCore.Mvc;
using SocPass.Application.Interface;
using SocPass.Domain.Model;

namespace SocPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : Controller
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }
        [HttpGet("get-all-userRole")]
        public async Task<IActionResult> GetAllUsers()
        {
            var usersRole = await _userRoleService.GetAllUsersRoleAsync();
            return Ok(usersRole);
        }
        [HttpPost("Create-User-Role")]
        public async Task<IActionResult> CreateMenuMaster([FromBody] UserRole userRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userRoleService.CreateUserRoleAsync(userRole);
            return Ok(result);
        }
        [HttpDelete("Delete-UserRole")]
        public async Task<IActionResult> Delete(int roleid)
        {
            try
            {
                await _userRoleService.DeleteUserRoleById(roleid);
                return Ok($"User with ID {roleid} has been deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"User Role with ID {roleid} not found: {ex.Message}");
            }
        }
        [HttpPut("Update-UserRole")]
        public async Task<IActionResult> UpdateUserAsync(int roleid, [FromBody] UserRole userRole)
        {
            if (roleid != userRole.UserRoleId)
            {
                return BadRequest("User Role ID mismatch.");
            }
            try
            {
                var updated = await _userRoleService.UpdateUserRoleAsync(roleid, userRole);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("GetUserRoleById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var role = await _userRoleService.GetUserRoleById(id);
                return Ok(role);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
