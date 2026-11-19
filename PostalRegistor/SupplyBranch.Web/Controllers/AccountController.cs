using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SupplyBranch.DataAccess;
using SupplyBranch.Web.Models;

namespace SupplyBranch.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserDAL _userDal;
        private readonly Microsoft.Extensions.Logging.ILogger<AccountController> _logger;

        public AccountController(UserDAL userDal, Microsoft.Extensions.Logging.ILogger<AccountController> logger)
        {
            _userDal = userDal;
            _logger = logger;
        }

        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Console.WriteLine($"[AccountController.Login] POST attempt for user: '{model?.UserName}'");
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = _userDal.Login(model.UserName, model.Password);

                if (user == null)
                {
                    // Specific invalid credentials message
                    ModelState.AddModelError(string.Empty, "Invalid Username, Password, or Account is Inactive.");
                    TempData["ErrorMessage"] = "Invalid Username, Password, or Account is Inactive.";
                    _logger.LogInformation("Login failed for user {UserName}", model.UserName);
                    Console.WriteLine($"[AccountController.Login] Login failed for '{model?.UserName}' (UserDAL returned null)");
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("FullName", user.FullName ?? string.Empty)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                Console.WriteLine($"[AccountController.Login] SignInAsync completed for '{user.UserName}'");

                // After successful sign-in, always redirect to the dashboard explicitly.
                return RedirectToAction("Index", "Home");
            }
            catch (InvalidOperationException ioex)
            {
                // Specific known issues (e.g., account inactive)
                _logger.LogWarning(ioex, "Login issue for user {UserName}: {Message}", model.UserName, ioex.Message);
                // Surface the exact issue to the user so they have immediate feedback
                ModelState.AddModelError(string.Empty, ioex.Message);
                TempData["ErrorMessage"] = ioex.Message;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {UserName}", model.UserName);
                // If a database/SQL error occurs, show the error message on the login page for debugging
                string msg = "An error occurred while processing your login: " + ex.Message;
                ModelState.AddModelError(string.Empty, msg);
                TempData["ErrorMessage"] = msg;
                return View(model);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
    }
}
