using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.AccountDTOs;
using Application.DTOs.AuthDTOs;
using Application.DTOs.CustomerDTOs;
using Application.Utilities.Response;
using Application.VMs;

namespace Application.Services
{
    public interface IAccountService
    {
        Task<GenericResponse<bool>> AddAccountToCustomerAsync(AddAccounToCustomerDTO model);
        Task<GenericResponse<bool>> DeleteAccountAsync(DeleteAccountDTO model);
        Task<GenericResponse<bool>> RemoveAccountAsync(DeleteAccountDTO model);
        Task<GenericResponse<string>> LoginAccountAsync(AccountLoginDTO model);
        Task<GenericResponse<string>> VerifyOTPAsync(CodeDTO model);
        Task<GenericResponse<bool>> RemoveAccountFromCustomerAsync(
            RemoveAccountFromCustomerDTO model
        );
        Task<GenericResponse<AccountVM>> GetBalance(GetBalanceDTO model);
        Task<GenericResponse<AccountInfoVM>> AccountInfo(AccountInfoDTO model);
        Task<GenericResponse<IbanListData>> GetIbanList(AccountInfoDTO model);
        Task<GenericResponse<TransactionHistoryData>> GetTransactionsAsync(
            AccountTransactionDTO model
        );
        Task<GenericResponse<SendMoneyResponseData>> SendMoneyAsync(SendMoneyDTO model);
        Task<GenericResponse<SendMoneyResponseData>> SendMoneyToMypayzAsync(
            SendMoneyToMypayzDTO model
        );
        Task<GenericResponse<VerifyMyPayzNumberData>> VerifyMypayzNumberAsync(
            VerifyMypayzNumberDTO model
        );
        Task<GenericResponse<string>> RequestMoneyQrAsync(RequestMoneyQrDTO model);
        Task<GenericResponse<bool>> RequestMoneyAsync(RequestMoneyDTO model);
        Task<GenericResponse<bool>> ActivateAccountAsync(DeleteAccountDTO model);
    }
}
