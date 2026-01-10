using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ValueTech.Api.Contracts.Requests;
using ValueTech.Web.Services;

namespace ValueTech.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApiClient _apiClient;

        public AccountController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid) return View(model);

            var isValid = await _apiClient.ValidateUserAsync(model.Username, model.Password);

            if (isValid)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = "Usuario o contraseña incorrectos.";
            return View(model);
        }

        [HttpPost] // Post para evitar ataques CSRF en Logout GET
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
