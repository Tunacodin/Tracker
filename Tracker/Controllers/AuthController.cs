using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Tracker.Api;
using Tracker.Models;
using Application.Utilities.Constants;  // For Roles

namespace Tracker.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService apiService;

        public AuthController(ApiService apiService)
        {
            this.apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginVM loginVM)
        {
            var response = await apiService.PostAsync<bool>("Auth/login", loginVM);

            if (response != null && response.IsSuccess)
            {
                TempData["Email"] = loginVM.Email;
                return RedirectToAction("VerifyCode");
            }

            ModelState.AddModelError("", $"Login failed: {response?.Message}");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> VerifyCode(TwoFactorAuthDTO twoFactorAuthDTO)
        {
            var response = await apiService.PostAsync<Token>("Auth/verify-code", twoFactorAuthDTO);

            if (response != null && response.IsSuccess && !string.IsNullOrWhiteSpace(response.Data.AccessToken))
            {
                // Token'ı session ve cookie'ye kaydet
                HttpContext.Session.SetString("AuthToken", response.Data.AccessToken);
                HttpContext.Response.Cookies.Append(
                    "AuthToken",
                    response.Data.AccessToken,
                    new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.Now.AddDays(1)
                    }
                );

                try
                {
                    // JWT token'ı çözümle
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(response.Data.AccessToken);
                    var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                    // Claims oluştur
                    var claims = new List<Claim>
                    {
                    new(ClaimTypes.NameIdentifier, response.Data.Id),
                    new("Token", response.Data.AccessToken),
                    new(ClaimTypes.Role, role ?? "Customer"),
                    new(ClaimTypes.Email, twoFactorAuthDTO.Email)
                    };

                    // Identity ve Principal oluştur
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Cookie authentication ile giriş yap
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Token çözümleme hatası: {ex.Message}");
                    return View();
                }
            }

            ModelState.AddModelError("", response?.Message ?? "Doğrulama başarısız");
            return View();
        }

        public IActionResult VerifyCode()
        {
            var email = TempData["Email"] as string;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index");
            }

            ViewBag.Email = email;
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            // Oturumdan çıkış
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            Response.Cookies.Delete("AuthToken");

            return RedirectToAction("Index", "Auth");
        }
    }
}
