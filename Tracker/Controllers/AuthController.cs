using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Tracker.Api;
using Tracker.Constants;
using Tracker.Models;

namespace Tracker.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService apiService;

        public AuthController(ApiService apiService)
        {
            this.apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            //await apiService.PostAsync<bool>("Log", new CreateMVCLogDTO { Page = MVCPages.Login });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginVM loginVM)
        {
            var response = await apiService.PostAsync<Token>("Auth", loginVM);

            if (response != null && response.IsSuccess)
            {
                HttpContext.Session.SetString("AuthToken", response.Data.AccessToken);
                HttpContext.Response.Cookies.Append(
                    "AuthToken",
                    response.Data.AccessToken,
                    new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.Now.AddDays(1),
                    }
                );
                // Decode the JWT token to get the role
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(response.Data.AccessToken);
                var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, response.Data.Id),
                    new Claim("Token", response.Data.AccessToken),
                    new Claim(ClaimTypes.Role, role ?? Roles.Customer),
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal
                );

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", $"Login failed : {response?.Message}");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await apiService.PostAsync<bool>("Log", new CreateMVCLogDTO { Page = MVCPages.Login });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            Response.Cookies.Delete("AuthToken");

            return RedirectToAction("Index", "Auth");
        }
    }
}
