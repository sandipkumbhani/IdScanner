using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocPass.Application.Interface;
using SocPass.Domain.Model;
using System.Data;

namespace SocPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("get-all-user")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] User user, int? flatId = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _userService.CreateUserAsync(user,flatId);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return Ok($"This email is already registered.");
            }

        }
        [HttpDelete("Delete-User")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteUserById(id);
                return Ok($"User with ID {id} has been deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"User with ID {id} not found: {ex.Message}");
            }
        }
        [HttpPut("Update-User/{userid}")]
        public async Task<IActionResult> UpdateUserAsync(int userid, [FromBody] User user)
        {
            var existingUser = _userService.GetUserDetailsById(userid);
            if (existingUser == null && userid != user.UserId)
            {
                return BadRequest("User ID mismatch.");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    var updatedUser = await _userService.UpdateUserAsync(userid, user);
                    return Ok(updatedUser);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = ex.Message });
                }
            }
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> UserGetById(int userid)
        {
            try
            {
                var result = await _userService.GetUserDetailsById(userid);
                if (result == null)
                {
                    return NotFound("User not found.");
                }

                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

    }
}
