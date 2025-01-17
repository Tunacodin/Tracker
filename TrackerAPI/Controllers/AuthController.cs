using Application.DTOs;
using Application.DTOs.AuthDTOs;
using Application.Services;
using Application.Utilities.Constants;
using Application.Utilities.Helper;
using Application.Utilities.Response;
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
            // Giriş bilgilerinin validasyonu
            LoginDTOValidator validator = new();
            var validationResult = validator.Validate(loginDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    IsSuccess = false,
                    Message = Messages.ValidationFailed,
                    ValidationErrors = validationResult.Errors.GetValidationErrors()
                });
            }

            // Login işlemi ve doğrulama kodu gönderme
            var response = await customerService.LoginCustomerAsync(loginDTO);

            if (!response.IsSuccess)
            {
                return Unauthorized(new GenericResponse<bool>
                {
                    IsSuccess = false,
                    Message = response.Message
                });
            }

            return Ok(new GenericResponse<bool>
            {
                IsSuccess = true,
                Message = Messages.VerificationCodeSent
            });
        }

        /// <summary>
        /// Kullanıcı doğrulama kodunu gönderir ve giriş işlemini tamamlar.
        /// </summary>
        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] TwoFactorAuthDTO twoFactorAuthDTO)
        {
            // Validasyon kontrolü
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<Token>
                {
                    IsSuccess = false,
                    Message = Messages.ValidationFailed,
                    ValidationErrors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            // Doğrulama kodu kontrolü ve token oluşturma işlemi
            var response = await customerService.VerifyLoginCodeAsync(twoFactorAuthDTO.Email, twoFactorAuthDTO.VerificationCode);

            if (!response.IsSuccess)
            {
                return Unauthorized(new GenericResponse<Token>
                {
                    IsSuccess = false,
                    Message = response.Message
                });
            }

            // Başarılı doğrulama durumunda token bilgisini dön
            return Ok(new GenericResponse<Token>
            {
                IsSuccess = true,
                Message = Messages.LoginSuccess,
                Data = response.Data
            });
        }
    }
}
