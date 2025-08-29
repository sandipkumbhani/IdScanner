using IdScanner.UI.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdScanner.UI.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly IForgotPasswordService _forgotPasswordService;
        public ForgotPasswordController(IForgotPasswordService forgotPasswordService)
        {
            _forgotPasswordService = forgotPasswordService
                ?? throw new ArgumentNullException(nameof(forgotPasswordService));
        }
        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View("~/Views/ForgotPassword/ForgotPassword.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Email is required";
                return View("ForgotPassword");
            }
            try
            {
                var result = await _forgotPasswordService.ForgotPasswordAsync(email);
                ViewBag.Message = "Password reset link has been sent.";
                return View("ForgotPassword");
            }
            catch(KeyNotFoundException ex)
            {
                ViewBag.Error = ex.Message;
                return View("ForgotPassword");
            }
            catch(Exception ex)
            {
                ViewBag.Error = "An unexpected error occurred: " + ex.Message;
                return View("ForgotPassword");
            }
        }
    }
}
