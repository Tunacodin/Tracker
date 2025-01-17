using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.AccountDTOs;
using Application.DTOs.AuthDTOs;
using Application.DTOs.CodeDTOs;
using Application.DTOs.CustomerDTOs;
using Application.Utilities.Response;
using Application.VMs;

namespace Application.Services
{
    public interface ICustomerService
    {

        // Yeni Ekleme
        Task<GenericResponse<bool>> VerifyLoginCodeAsync(string email, string verificationCode);
        Task<GenericResponse<Token>> CreateCustomerAsync(CreateCustomerDTO model);
        Task<GenericResponse<Token>> LoginCustomerAsync(LoginDTO model);
        Task<GenericResponse<BalancesVM>> GetBalancesAsync(string email);
        Task<GenericResponse<List<AccountVM>>> GetCustomerAccounts(string email);
        Task<GenericResponse<List<CustomerVM>>> GetAllCustomers();
        Task<GenericResponse<CodeVM>> TransferAccountToCustomer(
            string email,
            PhoneNumbers accounts
        );
        Task<GenericResponse<bool>> VerifyCode(VerifyCodeDTO model);
        Task<GenericResponse<bool>> DeleteCustomer(string email);
        Task<GenericResponse<bool>> MakeActiveCustomer(string email);
        Task<GenericResponse<PaginatedResponse<CustomerVM>>> SearchCustomer(
            string search,
            PaginationDTO pagination
        );
        Task<GenericResponse<BalancesVMPaginated>> GetBalancesAsyncPaginated(
            string email,
            PaginationDTO pagination
        );
        Task<GenericResponse<PaginatedResponse<CustomerVM>>> GetAllCustomersPaginated(
            PaginationDTO pagination
        );

        Task<GenericResponse<Token>> GenerateToken(string email);
    }
}
