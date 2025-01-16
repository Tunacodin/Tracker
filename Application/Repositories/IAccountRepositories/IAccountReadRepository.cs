using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Utilities.Response;
using Application.VMs;
using Domain.Entities;

namespace Application.Repositories.IAccountRepositories
{
    public interface IAccountReadRepository : IReadRepository<Account>
    {
        Task<List<Account>> GetAccountsByCustomerIdAsync(
            string customerId,
            bool asNoTracking = false
        );
        Task<Account> GetAccountByPhone(string phone);
        Task<PaginatedResponse<Account>> GetAccountsByCustomerIdAsyncPaginated(
            string customerId,
            PaginationDTO pagination,
            bool isAdmin = false
        );
        Task<Account> GetAccountByIdForAdmin(string id);
        Task<Account> GetAccountByPhoneForAdmin(string phone);
    }
}
