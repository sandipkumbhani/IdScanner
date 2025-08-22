using IdScanner.Application.Interface;
using IdScanner.Application.Services;
using IdScanner.Domain.DTO;
using IdScanner.Domain.Model;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ldScanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IUserServices _userLoginService;
        public LoginController(IUserServices userLoginService)
        {
            _userLoginService = userLoginService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.EmailId) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Email and Password are required" });
            }

            var result = await _userLoginService.LoginAsync(request.EmailId, request.Password);

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(result);
        }

    }
}
