using IdScanner.Application.Interface;
using IdScanner.Application.Services;
using IdScanner.Domain.DTO;
using IdScanner.Domain.Model;
using IdScanner.UI.Domain.Comman;
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
                return Unauthorized(new CommanResponseDto
                {
                    StatusCode = 401,
                    Message = "Unauthorized",
                    ErrorMessage = "Invalid Email or password"
                });
            }
            return Ok(new CommanResponseDto
            {
                StatusCode = 200,
                Message = "Login successful",
                Data = result
            });
        }

    }
}
