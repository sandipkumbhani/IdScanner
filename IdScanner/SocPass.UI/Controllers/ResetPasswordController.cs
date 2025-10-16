using Microsoft.AspNetCore.Mvc;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly IResetPasswordService _resetPasswordService;
        public ResetPasswordController(IResetPasswordService resetPasswordService)
        {
            _resetPasswordService = resetPasswordService;
        }
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            return View("/Views/ResetPassword/Resetpassword.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            string errorMessage = ValidatePassword(resetPasswordModel);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.PasswordError = errorMessage;
                return View("/Views/ResetPassword/Resetpassword.cshtml", resetPasswordModel);
            }

            var response = await _resetPasswordService.ResetPassworsdAsync(resetPasswordModel);
            if (response.Contains("success", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login", "Login");
            }
            return View("/Views/ResetPassword/Resetpassword.cshtml");
        }
        private string ValidatePassword(ResetPasswordModel resetPasswordModel)
        {
            if (resetPasswordModel == null)
            { 
                throw new ArgumentNullException(nameof(resetPasswordModel));
            }
            if (string.IsNullOrWhiteSpace(resetPasswordModel.NewPassword))
            {
                return "Password is required.";
            }
            if (resetPasswordModel.NewPassword.Length < 8)
            {
                return "Password must be at least 8 characters long.";
            }
            if (!resetPasswordModel.NewPassword.Any(char.IsUpper))
            {
                return "Password must contain at least one uppercase letter.";
            }
            if (!resetPasswordModel.NewPassword.Any(char.IsLower))
            {
                return "Password must contain at least one lowercase letter.";
            }
            if (!resetPasswordModel.NewPassword.Any(char.IsDigit))
            {
                return "Password must contain at least one number.";
            }
            if (!resetPasswordModel.NewPassword.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                return "Password must contain at least one special character.";
            }
            if (resetPasswordModel.NewPassword != resetPasswordModel.ConfirmPassword)
            {
                return "Passwords do not match.";
            }
            return null;
        }
    }
}
