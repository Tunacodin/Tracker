using Application.DTOs;
using Application.Repositories.IAccountRepositories;
using Application.Utilities.Response;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.AccountRepository
{
    public class AccountReadRepository : ReadRepository<Account>, IAccountReadRepository
    {
        private readonly TrackerDbContext db;

        public AccountReadRepository(TrackerDbContext db)
            : base(db)
        {
            this.db = db;
        }

        public async Task<List<Account>> GetAccountsByCustomerIdAsync(
            string customerId,
            bool asNoTracking = false
        )
        {
            var query = asNoTracking ? db.Accounts.AsNoTracking() : db.Accounts;
            return await query
                .Include(a => a.Customer)
                .Where(a =>
                    a.Customer.Any(ca => ca.Id == customerId)
                    && a.Status != Domain.Enums.Status.Pasive
                )
                .ToListAsync();
        }

        public async Task<PaginatedResponse<Account>> GetAccountsByCustomerIdAsyncPaginated(
            string customerId,
            PaginationDTO pagination,
            bool isAdmin = false
        )
        {
            var query = db
                .Accounts.Include(a => a.Customer)
                .Where(a =>
                    (isAdmin ? true : a.Customer.Any(ca => ca.Id == customerId))
                    && (isAdmin ? true : a.Status != Domain.Enums.Status.Pasive)
                );
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);
            var accounts = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
            return new PaginatedResponse<Account>
            {
                Data = accounts,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = totalPages,
                TotalCount = totalCount,
            };
        }

        public async Task<Account> GetAccountByPhone(string phone)
        {
            var result = await db
                .Accounts.Include(a =>
                    a.Customer.Where(c => c.Status != Domain.Enums.Status.Pasive)
                )
                .Where(a => a.Phone == phone && a.Status != Domain.Enums.Status.Pasive)
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<Account> GetAccountByIdForAdmin(string id)
        {
            return await db.Accounts.FindAsync(id);
        }

        public async Task<Account> GetAccountByPhoneForAdmin(string phone)
        {
            return await db.Accounts.FirstOrDefaultAsync(a => a.Phone == phone.Trim());
        }
    }
}
