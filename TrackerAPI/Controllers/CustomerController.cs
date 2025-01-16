using Application.DTOs;
using Application.DTOs.AccountDTOs;
using Application.DTOs.CodeDTOs;
using Application.DTOs.CustomerDTOs;
using Application.Services;
using Application.Utilities.Constants;
using Application.Utilities.Helper;
using Application.Utilities.Response;
using Application.VMs;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.ConcreteServices.CustomerService;

namespace TrackerAPI.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService, ILoggerService loggerService)
            : base(loggerService)
        {
            this.customerService = customerService;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDTO createCustomerDTO)
        {
            await LogApiActionAsync(
                $"Musteri olusturma denemesi hesap: {HttpContext.Items["Email"]} - olusturulacak hesap: {createCustomerDTO.Email}",
                ActionType.CreateCustomer,
                ProcessStatus.Started,
                createCustomerDTO.Email
            );

            CreateCustomerDTOValidator validator = new();
            var validationResult = validator.Validate(createCustomerDTO);

            GenericResponse<Token> response = new();

            if (validationResult.IsValid)
            {
                // Kullanıcıyı oluşturan kişinin emailini ayarla
                createCustomerDTO.WhoAdded = HttpContext.Items["Email"] as string;

                // Müşteri oluşturma servisini çağır
                response = await customerService.CreateCustomerAsync(createCustomerDTO);
            }
            else
            {
                // Validasyon hatalarını ekle
                response.ValidationErrors = validationResult.Errors.GetValidationErrors();
                response.Message = Messages.ValidationFailed;
                response.IsSuccess = false;
            }

            await LogResultAsync(
                response,
                ActionType.CreateCustomer,
                "Musteri olusturma denemesi",
                createCustomerDTO.Email
            );

            // Yanıtı döndür
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("Dashboard")]
        public async Task<IActionResult> GetDashboard(string? email, PaginationDTO pagination)
        {
            await LogApiActionAsync(
                $"Anasayfa denemesi hesap: {HttpContext.Items["Email"]}",
                ActionType.GetBalances,
                ProcessStatus.Started,
                email
            );

            PaginationDTOValidator validator = new();
            var result = validator.Validate(pagination);
            GenericResponse<BalancesVMPaginated> response = new();

            if (result.IsValid)
            {
                response = await customerService.GetBalancesAsyncPaginated(
                    string.IsNullOrEmpty(email) ? HttpContext.Items["Email"] as string : email,
                    pagination
                );
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.Message = Messages.ValidationFailed;
                response.IsSuccess = false;
            }

            await LogResultAsync(response, ActionType.GetBalances, "Anasayfa denemesi", email);
            return Ok(response);
        }

        [HttpGet("Customer-Accounts")]
        public async Task<IActionResult> GetCustomerAccounts()
        {
            await LogApiActionAsync(
                $"Musteri hesaplari sorgusu hesap: {HttpContext.Items["Email"]}",
                ActionType.GetCustomerAccounts,
                ProcessStatus.Started
            );

            GenericResponse<List<AccountVM>> response = await customerService.GetCustomerAccounts(
                HttpContext.Items["Email"] as string
            );

            await LogResultAsync(
                response,
                ActionType.GetCustomerAccounts,
                "Musteri hesaplari sorgusu"
            );
            return Ok(response);
        }

        [HttpPost("All-Customers")]
        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> GetAllCustomers(PaginationDTO pagination)
        {
            await LogApiActionAsync(
                $"Tum musteriler sorgusu hesap: {HttpContext.Items["Email"]}",
                ActionType.GetAllCustomers,
                ProcessStatus.Started
            );

            PaginationDTOValidator validator = new();
            var result = validator.Validate(pagination);
            GenericResponse<PaginatedResponse<CustomerVM>> response = new();

            if (result.IsValid)
            {
                response = await customerService.GetAllCustomersPaginated(pagination);
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.Message = Messages.ValidationFailed;
                response.IsSuccess = false;
            }

            await LogResultAsync(response, ActionType.GetAllCustomers, "Tum musteriler sorgusu");
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> GetAllCustomersWithOutPagination()
        {
            await LogApiActionAsync(
                $"Tum musteriler sorgusu (paginationsiz) hesap: {HttpContext.Items["Email"]}",
                ActionType.GetAllCustomers,
                ProcessStatus.Started
            );

            GenericResponse<List<CustomerVM>> response = await customerService.GetAllCustomers();

            await LogResultAsync(
                response,
                ActionType.GetAllCustomers,
                "Tum musteriler sorgusu (paginationsiz)"
            );
            return Ok(response);
        }

        [HttpPost("Code")]
        public async Task<IActionResult> GetCode(
            [FromQuery] string? email,
            [FromBody] PhoneNumbers accounts
        )
        {
            await LogApiActionAsync(
                $"Kod talebi hesap: {HttpContext.Items["Email"]} - hedef hesap: {email}",
                ActionType.TransferAccountToCustomer,
                ProcessStatus.Started,
                email
            );

            GenericResponse<CodeVM> response = new();
            response = await customerService.TransferAccountToCustomer(
                string.IsNullOrEmpty(email) ? HttpContext.Items["Email"] as string : email,
                accounts
            );

            await LogResultAsync(response, ActionType.TransferAccountToCustomer, "Kod talebi");
            return Ok(response);
        }

        [HttpPost("Verify-Code")]
        public async Task<IActionResult> VerifyCode(VerifyCodeDTO model)
        {
            await LogApiActionAsync(
                $"Kod dogrulama hesap: {HttpContext.Items["Email"]} - hedef hesap: {model.CustomerEmail}",
                ActionType.VerifyCode,
                ProcessStatus.Started,
                model.CustomerEmail
            );

            VerifyCodeDTOValidator validator = new();
            model.CustomerEmail = model.CustomerEmail ?? HttpContext.Items["Email"] as string;
            var result = validator.Validate(model);
            GenericResponse<bool> response = new();
            if (result.IsValid)
            {
                response = await customerService.VerifyCode(model);
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.Message = Messages.ValidationFailed;
                response.IsSuccess = false;
            }
            return Ok(response);
        }

        [HttpDelete]
        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> DeleteCustomer(string email)
        {
            await LogApiActionAsync(
                $"Musteri silme islemi hesap: {HttpContext.Items["Email"]} - silinecek hesap: {email}",
                ActionType.DeleteCustomer,
                ProcessStatus.Started,
                email
            );

            GenericResponse<bool> response = new();
            if (string.IsNullOrEmpty(email))
            {
                response.IsSuccess = false;
                response.Message = Messages.EmailRequired;
            }
            else
            {
                response = await customerService.DeleteCustomer(email);
            }

            await LogResultAsync(
                response,
                ActionType.DeleteCustomer,
                "Musteri silme islemi",
                email
            );
            return Ok(response);
        }

        [HttpPost("Make-Active")]
        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> MakeActiveCustomer(string email)
        {
            await LogApiActionAsync(
                $"Musteri aktivasyon islemi hesap: {HttpContext.Items["Email"]} - aktive edilecek hesap: {email}",
                ActionType.MakeActiveCustomer,
                ProcessStatus.Started,
                email
            );

            GenericResponse<bool> response = new();
            if (string.IsNullOrEmpty(email))
            {
                response.IsSuccess = false;
                response.Message = Messages.EmailRequired;
            }
            else
            {
                response = await customerService.MakeActiveCustomer(email);
            }

            await LogResultAsync(
                response,
                ActionType.MakeActiveCustomer,
                "Musteri aktivasyon islemi",
                email
            );
            return Ok(response);
        }

        [HttpPost("Search-Customer")]
        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> SearchCustomer(string search, PaginationDTO pagination)
        {
            await LogApiActionAsync(
                $"Musteri arama islemi hesap: {HttpContext.Items["Email"]} - Arama: {search}",
                ActionType.SearchCustomer,
                ProcessStatus.Started
            );

            GenericResponse<PaginatedResponse<CustomerVM>> response = new();
            PaginationDTOValidator validator = new();
            var result = validator.Validate(pagination);

            if (!result.IsValid)
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.Message = Messages.ValidationFailed;
                response.IsSuccess = false;
            }
            else if (string.IsNullOrEmpty(search))
            {
                response.IsSuccess = false;
                response.Message = Messages.SearchFailed;
            }
            else
            {
                response = await customerService.SearchCustomer(search, pagination);
            }

            await LogResultAsync(response, ActionType.SearchCustomer, "Musteri arama islemi");
            return Ok(response);
        }
    }
}
