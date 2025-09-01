using IdScanner.Application.Interface;
using IdScanner.Application.Services;
using IdScanner.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace ldScanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController : Controller
    {
        private readonly IUserDataService _userDataService;
        public UserDataController(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }
        [HttpGet("Get-All-User-Data")]
        public async Task<IActionResult> GetAllUserDataAsync()
        {

            var users = await _userDataService.GetAllUsersListAsync();
            return Ok(users);
        }
        [HttpGet("GetByUserDataById")]
        public async Task<IActionResult> GetById(int userid)
        {
            try
            {
                var userData = await _userDataService.GetUserDetailsById(userid);
                return Ok(userData);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateUserDataAsync([FromBody] UserData userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _userDataService.CreateUserDataAsync(userData);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"This User is not registered.");
            }
        }
        [HttpPut("Update-UserData/{userid}")]
        public async Task<IActionResult> UpdateUserDataAsync(int userid, [FromBody] UserData userData)
        {
            var existingUser = await _userDataService.GetUserDetailsById(userid);
            if (existingUser == null && userid != userData.UserDataId)
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
                    var updatedUser = await _userDataService.UpdateUserDataAsync(userid, userData);
                    return Ok(updatedUser);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = ex.Message });
                }
            }
        }
        [HttpDelete("Delete-User-Data")]
        public async Task<IActionResult> Delete(int userId)
        {
            try
            {
                await _userDataService.DeleteUserDataById(userId);
                return Ok($"User with ID {userId} has been deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"User with ID {userId} not found: {ex.Message}");
            }
        }


    }

}
