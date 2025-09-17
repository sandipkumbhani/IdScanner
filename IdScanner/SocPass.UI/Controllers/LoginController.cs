using IdScanner.UI.Domain.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using System.Security.Claims;

namespace SocPass.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginServices _loginServices;
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;

        public LoginController(ILoginServices loginServices, IConfiguration configuration)
        {
            _loginServices = loginServices;
            _configuration = configuration;
            applicationURL = new ApplicationURL(configuration);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Login/Login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var responseToken = await _loginServices.Login(viewModel);
                    Response.Cookies.Append("jwtToken", responseToken.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddHours(24)
                    });
                    var claims = new List<Claim>
                    {
                        new Claim("UserId",responseToken.UserId.ToString()),
                        new Claim(ClaimTypes.Name, responseToken.UserName),
                        new Claim(ClaimTypes.Email, responseToken.EmailId),
                        //new Claim(ClaimTypes.Role, responseToken.UserRoleName)
                    };
                    var identity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               principal,
               new AuthenticationProperties
                {
                   IsPersistent = true,
                   ExpiresUtc = DateTime.UtcNow.AddHours(24)
                });

                    return Redirect("~/MenuMaster/MenuMasterList");
                }
                else
                {
                    ViewBag.LoginMessage = "";
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                ViewBag.LoginMessage = ex.Message;
                return View(viewModel);
            }
        }
    }
}
