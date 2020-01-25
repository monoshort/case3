using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace BackOfficeFrontendService.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            if(!HttpContext.User.Identity.IsAuthenticated)
            {
                return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Logout()
        {
            return new SignOutResult(new[]
            {
                OpenIdConnectDefaults.AuthenticationScheme,
                CookieAuthenticationDefaults.AuthenticationScheme
            });
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            return View("AccessDenied", returnUrl);
        }
    }
}
