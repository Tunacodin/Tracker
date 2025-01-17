using Application.DTOs;
using Application.DTOs.AuthDTOs;
using Application.Services;
using Application.Utilities.Constants;
using Application.Utilities.Helper;
using Application.Utilities.Response;
using Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace TrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public AuthController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        /// <summary>
        /// Kullanıcı giriş yapar ve doğrulama kodu gönderilir.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            LoginDTOValidator validator = new();
            var validationResult = validator.Validate(loginDTO);
            GenericResponse<Token> response = new();

            if (!validationResult.IsValid)
            {
                response.ValidationErrors = validationResult.Errors.GetValidationErrors();

                response.Message = Messages.ValidationFailed;
                response.IsSuccess = false;
                return BadRequest(response);
            }

            response = await customerService.LoginCustomerAsync(loginDTO);

            if (!response.IsSuccess)
                return Unauthorized(response);

            return Ok(response);
        }

        /// <summary>
        /// Kullanıcı doğrulama kodunu gönderir ve giriş işlemini tamamlar.
        /// </summary>
        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] TwoFactorAuthDTO twoFactorAuthDTO)
        {
            // Model State kontrolü (Validasyon hatalarını kontrol et)
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    IsSuccess = false,
                    Message = Messages.ValidationFailed,
                    ValidationErrors = ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            // Doğrulama kodunu kontrol et
            var response = await customerService.VerifyLoginCodeAsync(
                twoFactorAuthDTO.Email,
                twoFactorAuthDTO.VerificationCode
            );

            if (!response.IsSuccess)
            {
                // Eğer doğrulama başarısızsa, 401 Unauthorized yanıtı döndür
                return Unauthorized(response);
            }

            // Eğer doğrulama başarılıysa, JWT token oluştur
            var tokenResponse = await customerService.GenerateToken(twoFactorAuthDTO.Email.ToString());

            if (tokenResponse == null || !tokenResponse.IsSuccess)
            {
                // Token üretimi başarısızsa hata yanıtı döndür
                return StatusCode(500, new GenericResponse<bool>
                {
                    IsSuccess = false,
                    Message = "Token oluşturulurken bir hata oluştu."
                });
            }

            // Yanıta token bilgisini ekle
            return Ok(new GenericResponse<Token>
            {
                IsSuccess = true,
                Message = "Doğrulama başarılı. Token oluşturuldu.",
                Data = tokenResponse.Data
            });
        }

    }
}
