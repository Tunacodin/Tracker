using Application.DTOs;
using Application.DTOs.AuthDTOs;
using Application.Services;
using Application.Utilities.Constants;
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
                response.ValidationErrors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
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

            var response = await customerService.VerifyLoginCodeAsync(
                twoFactorAuthDTO.Email,
                twoFactorAuthDTO.VerificationCode
            );

            if (!response.IsSuccess)
                return Unauthorized(response);

            return Ok(response);
        }
    }
}
