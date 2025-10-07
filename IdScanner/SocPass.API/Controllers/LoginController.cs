using Microsoft.AspNetCore.Mvc;
using SocPass.Application.Interface;
using SocPass.Domain.DTO;

namespace SocPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.EmailId) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new { message = "Email and Password are required" });
                }

                var result = await _loginService.LoginAsync(request.EmailId, request.Password);

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
            catch (Exception ex)
            {
                return BadRequest(new CommanResponseDto
                {
                    StatusCode = 400,
                    Message = "Login failed",
                    ErrorMessage = ex.Message
                });
            }
        }


    }
}
