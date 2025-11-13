using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace SocPass.UI.Filters
{
    public class AuthorizeTokenAttribute : ActionFilterAttribute
    {
        private readonly string[] _allowedRoles;

        public AuthorizeTokenAttribute(params string[] roles)
        {
            _allowedRoles = roles ?? Array.Empty<string>();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        { 
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            if ((controller == "Login" || controller == "ForgotPassword" || controller == "ResetPassword") &&
                (action == "Login" || action == "Logout" || action == "ForgotPassword" || action == "ResetPassword"))
            {
                base.OnActionExecuting(context);
                return;
            }
            var token = context.HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectToActionResult("Login", "Login", null);
                return;
            }
            var role = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (_allowedRoles.Length > 0 && !_allowedRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "AccessDenied", null);
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
