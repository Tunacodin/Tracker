using Application.DTOs.AccountDTOs;
using Application.DTOs.AuthDTOs;
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
    public class AccountController : BaseController
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService, ILoggerService loggerService)
            : base(loggerService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> AccountLogin(AddAccounToCustomerDTO dto)
        {
            await LogApiActionAsync(
                $"Account olusturma veya login denemesi hesap: {HttpContext.Items["Email"]} - kullanilan account: {dto.Phone} - musteri: {dto.CustomerId}",
                ActionType.AddAccountToCustomer,
                ProcessStatus.Started,
                dto.CustomerId,
                dto.Phone
            );
            AddAccounToCustomerDTOValidator validator = new();
            var result = validator.Validate(dto);
            GenericResponse<bool> response = new();
            if (result.IsValid)
            {
                response = await accountService.AddAccountToCustomerAsync(dto);
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.IsSuccess = false;
            }
            await LogResultAsync(
                response,
                ActionType.AddAccountToCustomer,
                "Account olusturma veya login denemesi",
                dto.CustomerId,
                dto.Phone
            );
            return Ok(response);
        }

        [HttpPost("Verify")]
        public async Task<IActionResult> Code(CodeDTO codeDTO)
        {
            await LogApiActionAsync(
                $"Account verify denemesi hesap: {HttpContext.Items["Email"]} - kullanilan account: {codeDTO.Phone} - musteri: {codeDTO.CustomerId}",
                ActionType.VerifyOTP,
                ProcessStatus.Started,
                codeDTO.CustomerId,
                codeDTO.Phone
            );
            CodeDTOValidator validator = new();
            var result = validator.Validate(codeDTO);
            GenericResponse<string> response = new();
            if (result.IsValid)
            {
                response = await accountService.VerifyOTPAsync(codeDTO);
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.IsSuccess = false;
            }
            await LogResultAsync(
                response,
                ActionType.VerifyOTP,
                "Account verify denemesi",
                codeDTO.CustomerId,
                codeDTO.Phone
            );
            return Ok(response);
        }

        // [HttpDelete]
        // [Authorize(Roles = Roles.Master)]
        // public async Task<IActionResult> Delete(DeleteAccountDTO codeDTO)
        // {
        //     DeleteAccountDTOValidator validator = new();
        //     var result = validator.Validate(codeDTO);
        //     GenericResponse<bool> response = new();
        //     if (result.IsValid)
        //     {
        //         response = await accountService.DeleteAccountAsync(codeDTO);
        //     }
        //     else
        //     {
        //         response.ValidationErrors = result.Errors.GetValidationErrors();
        //         response.IsSuccess = false;
        //     }
        //     await LogResultAsync(
        //         response,
        //         ActionType.RemoveAccount,
        //         "Account silme denemesi",
        //         null,
        //         codeDTO.Phone
        //     );
        //     return Ok(response);
        // }

        [HttpDelete("Remove")]
        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> Remove(DeleteAccountDTO codeDTO)
        {
            await LogApiActionAsync(
                $"Account silme denemesi hesap: {HttpContext.Items["Email"]} - silinecek account: {codeDTO.Phone}",
                ActionType.RemoveAccount,
                ProcessStatus.Started,
                null,
                codeDTO.Phone
            );
            DeleteAccountDTOValidator validator = new();
            var result = validator.Validate(codeDTO);
            GenericResponse<bool> response = new();
            if (result.IsValid)
            {
                response = await accountService.RemoveAccountAsync(codeDTO);
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.IsSuccess = false;
            }
            await LogResultAsync(
                response,
                ActionType.RemoveAccount,
                "Account silme denemesi",
                null,
                codeDTO.Phone
            );
            return Ok(response);
        }

        [HttpPost("Activate-Account")]
        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> ActivateAccount(DeleteAccountDTO codeDTO)
        {
            await LogApiActionAsync(
                $"Account aktivasyon denemesi hesap: {HttpContext.Items["Email"]} - aktivasyon yapilacak account: {codeDTO.Phone}",
                ActionType.ActivateAccount,
                ProcessStatus.Started,
                null,
                codeDTO.Phone
            );
            DeleteAccountDTOValidator validator = new();
            var result = validator.Validate(codeDTO);
            GenericResponse<bool> response = new();
            if (result.IsValid)
            {
                response = await accountService.ActivateAccountAsync(codeDTO);
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.IsSuccess = false;
            }
            await LogResultAsync(
                response,
                ActionType.ActivateAccount,
                "Account aktivasyon denemesi",
                null,
                codeDTO.Phone
            );
            return Ok(response);
        }

        [HttpDelete("Remove-From-Customer")]
        public async Task<IActionResult> RemoveFromCustomer(RemoveAccountFromCustomerDTO codeDTO)
        {
            await LogApiActionAsync(
                $"Account silme denemesi hesap: {HttpContext.Items["Email"]} - silinecek account: {codeDTO.Phone} - musteri: {codeDTO.CustomerId}",
                ActionType.RemoveAccountFromCustomer,
                ProcessStatus.Started,
                codeDTO.CustomerId,
                codeDTO.Phone
            );
            RemoveAccountFromCustomerDTOValidator validator = new();
            var result = validator.Validate(codeDTO);
            GenericResponse<bool> response = new();
            if (result.IsValid)
            {
                response = await accountService.RemoveAccountFromCustomerAsync(codeDTO);
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.IsSuccess = false;
            }
            await LogResultAsync(
                response,
                ActionType.RemoveAccountFromCustomer,
                "Account silme denemesi",
                codeDTO.CustomerId,
                codeDTO.Phone
            );
            return Ok(response);
        }

        [HttpGet("Info")]
        public async Task<IActionResult> GetInfo(string phone)
        {
            await LogApiActionAsync(
                $"Account bilgi alma denemesi hesap: {HttpContext.Items["Email"]} - kullanilan account: {phone}",
                ActionType.AccountInfo,
                ProcessStatus.Started,
                null,
                phone
            );
            GenericResponse<AccountInfoVM> response = new();
            AccountInfoDTOValidator validator = new();
            var result = validator.Validate(new AccountInfoDTO() { Phone = phone });

            if (result.IsValid)
            {
                response = await accountService.AccountInfo(new AccountInfoDTO() { Phone = phone });
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.IsSuccess = false;
            }
            await LogResultAsync(
                response,
                ActionType.AccountInfo,
                "Account bilgi alma denemesi",
                null,
                phone
            );
            return Ok(response);
        }

        [HttpGet("Iban-List")]
        public async Task<IActionResult> GetIbanList(string phone)
        {
            await LogApiActionAsync(
                $"Account iban listesi alma denemesi hesap: {HttpContext.Items["Email"]} - kullanilan account: {phone}",
                ActionType.GetIbanList,
                ProcessStatus.Started,
                null,
                phone
            );
            GenericResponse<IbanListData> response = new();
            AccountInfoDTOValidator validator = new();
            var result = validator.Validate(new AccountInfoDTO() { Phone = phone });

            if (result.IsValid)
            {
                response = await accountService.GetIbanList(new AccountInfoDTO() { Phone = phone });
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.IsSuccess = false;
            }
            await LogResultAsync(
                response,
                ActionType.GetIbanList,
                "Account iban listesi alma denemesi",
                null,
                phone
            );
            return Ok(response);
        }

        [HttpPost("Transactions")]
        public async Task<IActionResult> GetTransactions(AccountTransactionDTO dto)
        {
            await LogApiActionAsync(
                $"Account transaction alma denemesi hesap: {HttpContext.Items["Email"]} - kullanilan account: {dto.Phone}",
                ActionType.GetTransactions,
                ProcessStatus.Started,
                null,
                dto.Phone
            );
            GenericResponse<TransactionHistoryData> response = new();
            AccountTransactionDTOValidator validator = new();
            var result = validator.Validate(dto);

            if (result.IsValid)
            {
                response = await accountService.GetTransactionsAsync(dto);
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.IsSuccess = false;
            }
            await LogResultAsync(
                response,
                ActionType.GetTransactions,
                "Account transaction alma denemesi",
                null,
                dto.Phone
            );
            return Ok(response);
        }

        [HttpPost("Send-Money")]
        public async Task<IActionResult> SendMoney(SendMoneyDTO dto)
        {
            await LogApiActionAsync(
                $"Account para gonderme denemesi hesap: {HttpContext.Items["Email"]} - kullanilan account: {dto.Phone} - gonderilecek miktar: {dto.Amount} - gonderilecek IBAN: {dto.IBAN}",
                ActionType.SendMoney,
                ProcessStatus.Started,
                null,
                dto.Phone
            );
            GenericResponse<SendMoneyResponseData> response = new();
            SendMoneyDTOValidator validator = new();
            var result = validator.Validate(dto);

            if (result.IsValid)
            {
                response = await accountService.SendMoneyAsync(dto);
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.IsSuccess = false;
            }
            await LogResultAsync(
                response,
                ActionType.SendMoney,
                "Account para gonderme denemesi",
                null,
                dto.Phone
            );
            return Ok(response);
        }

        [HttpPost("Send-Money-To-Mypayz")]
        public async Task<IActionResult> SendMoneyToMypayz(SendMoneyToMypayzDTO dto)
        {
            await LogApiActionAsync(
                $"Account para gonderme denemesi (Mypayz) hesap: {HttpContext.Items["Email"]} - kullanilan account: {dto.Phone} - gonderilecek miktar: {dto.Amount} - gonderilecek Mypayz numarasi: {dto.ReceiverMypayzNumber}",
                ActionType.SendMoneyToMypayz,
                ProcessStatus.Started,
                null,
                dto.Phone
            );
            GenericResponse<SendMoneyResponseData> response = new();
            SendMoneyToMypayzDTOValidator validator = new();
            var result = validator.Validate(dto);

            if (result.IsValid)
            {
                response = await accountService.SendMoneyToMypayzAsync(dto);
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.IsSuccess = false;
            }
            await LogResultAsync(
                response,
                ActionType.SendMoneyToMypayz,
                "Account para gonderme denemesi (Mypayz)",
                null,
                dto.Phone
            );
            return Ok(response);
        }

        [HttpPost("Verify-Mypayz-Number")]
        public async Task<IActionResult> VerifyMypayzNumber(VerifyMypayzNumberDTO dto)
        {
            await LogApiActionAsync(
                $"Account mypayz numarasini dogrulama denemesi hesap: {HttpContext.Items["Email"]} - kullanilan account: {dto.Phone}",
                ActionType.VerifyMypayzNumber,
                ProcessStatus.Started,
                null,
                dto.Phone
            );
            GenericResponse<VerifyMyPayzNumberData> response = new();
            VerifyMypayzNumberDTOValidator validator = new();
            var result = validator.Validate(dto);
            if (result.IsValid)
            {
                response = await accountService.VerifyMypayzNumberAsync(dto);
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.IsSuccess = false;
            }
            await LogResultAsync(
                response,
                ActionType.VerifyMypayzNumber,
                "Account mypayz numarasini dogrulama denemesi",
                null,
                dto.Phone
            );
            return Ok(response);
        }

        [HttpPost("Request-Money-Qr")]
        public async Task<IActionResult> RequestMoneyQr(RequestMoneyQrDTO dto)
        {
            await LogApiActionAsync(
                $"Account para istegi denemesi hesap: {HttpContext.Items["Email"]} - kullanilan account: {dto.Phone} - istenen miktar: {dto.Amount}",
                ActionType.RequestMoneyQr,
                ProcessStatus.Started,
                null,
                dto.Phone
            );
            GenericResponse<string> response = new();
            RequestMoneyQrDTOValidator validator = new();
            var result = validator.Validate(dto);
            if (result.IsValid)
            {
                response = await accountService.RequestMoneyQrAsync(dto);
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.IsSuccess = false;
            }
            await LogResultAsync(
                response,
                ActionType.RequestMoneyQr,
                "Account para istegi denemesi",
                null,
                dto.Phone
            );
            return Ok(response);
        }

        [HttpPost("Request-Money")]
        public async Task<IActionResult> RequestMoney(RequestMoneyDTO dto)
        {
            await LogApiActionAsync(
                $"Account para istegi denemesi hesap: {HttpContext.Items["Email"]} - kullanilan account: {dto.Phone} - istenen miktar: {dto.Amount} - istenen mypayz numarasi: {dto.FromWho}",
                ActionType.RequestMoney,
                ProcessStatus.Started,
                null,
                dto.Phone
            );
            GenericResponse<bool> response = new();
            RequestMoneyDTOValidator validator = new();
            var result = validator.Validate(dto);
            if (result.IsValid)
            {
                response = await accountService.RequestMoneyAsync(dto);
            }
            else
            {
                response.ValidationErrors = result.Errors.GetValidationErrors();
                response.IsSuccess = false;
            }
            await LogResultAsync(
                response,
                ActionType.RequestMoney,
                "Account para istegi denemesi",
                null,
                dto.Phone
            );
            return Ok(response);
        }
    }
}
