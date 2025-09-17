using IdScanner.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using SocPass.Application.Interface;
using SocPass.Domain.Model;
using SocPass.Infrastructure.Data;

namespace ldScanner.API.Controllers
{
    [Route("api/[controller]")]
        public class ForgotPasswordController : Controller
        {
            private readonly IForgotPasswordService _forgotPasswordService;
            private readonly IEmailService _emailService;
            private readonly AppDbContext _context;
            public ForgotPasswordController(IForgotPasswordService forgotPasswordService, IEmailService emailService, AppDbContext context)
            {
                _forgotPasswordService = forgotPasswordService;
                _emailService = emailService;
                _context = context;
            }
            [HttpGet("forgot-password")]
            public async Task<IActionResult> ForgotPassword(string email)
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("Email is required.");
                }
                var user = await _forgotPasswordService.CheckEmailidAsync(email);

                if (user == null)
                {
                    return Ok("If the email is registered, a reset link has been sent.");
                }
                var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                var expiry = DateTime.UtcNow.AddHours(1);
                var encodedToken = System.Web.HttpUtility.UrlEncode(token);
                var resetUrl = $"http://localhost:5109/ResetPassword/ResetPassword?email={email}&token={encodedToken}";
                try
                {
                    await _emailService.SendEmailAsync(
                        user.EmailId,
                        "Reset your password",
                        $"Click here to reset your password: {resetUrl}\nThis link will expire in 1 hour."
                    );
                }
                catch (Exception ex)
                {
                    return BadRequest($"Failed to send email: {ex.Message}");
                }
                return Content("Password reset link has been sent.");
            }
            [HttpPut("reset-password")]
            public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO resetPasswordRequestDTO)
            {
                var user = await _forgotPasswordService.CheckEmailidAsync(resetPasswordRequestDTO.Email);
                if (user == null)
                {
                    return NotFound($"User with email {resetPasswordRequestDTO.Email} not found.");
                }
                try
                {
                    var modelUser = new User
                    {
                        Password = resetPasswordRequestDTO.NewPassword
                    };

                    await _forgotPasswordService.UpdatePasswordAsync(resetPasswordRequestDTO.Email, modelUser);

                    return Ok(new { message = "Password reset successfully." });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = ex.Message });
                }
            }
        }
}
