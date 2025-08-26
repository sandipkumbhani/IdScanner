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

    }

}
