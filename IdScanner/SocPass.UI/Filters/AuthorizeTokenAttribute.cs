using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SocPass.UI.Filters
{
    public class AuthorizeTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();
            if (controller == "Login" &&
                (action == "Login" || action == "Logout" || action == "ForgotPassword"))
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

            base.OnActionExecuting(context);
        }
    }
}

