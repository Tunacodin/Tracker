using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Repositories.ICustomerRepositories;
using Application.Utilities.Response;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.CustomerRepository
{
    public class CustomerReadRepository : ReadRepository<Customer>, ICustomerReadRepository
    {
        private readonly TrackerDbContext db;

        public CustomerReadRepository(TrackerDbContext db)
            : base(db)
        {
            this.db = db;
        }

        public async Task<Customer> GetCustomer(string id)
        {
            return await db
                .Customers.Include(a =>
                    a.Accounts.Where(acc => acc.Status != Domain.Enums.Status.Pasive)
                )
                .FirstOrDefaultAsync(a => a.Id == id && a.Status != Domain.Enums.Status.Pasive);
        }

        public async Task<Customer> GetCustomerByEmail(string email)
        {
            return await db
                .Customers.Include(a =>
                    a.Accounts.Where(acc => acc.Status != Domain.Enums.Status.Pasive)
                )
                .FirstOrDefaultAsync(a =>
                    a.Email == email && a.Status != Domain.Enums.Status.Pasive
                );
        }

        public async Task<PaginatedResponse<Customer>> SearchCustomerPaginated(
            string search,
            PaginationDTO pagination
        )
        {
            var query = db.Customers.AsQueryable();
            // Apply search if provided
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(c =>
                    c.FullName.ToLower().Contains(search)
                    || c.Email.ToLower().Contains(search)
                    || c.PhoneNumber.Contains(search)
                    || c.Id.ToLower().Contains(search)
                    || c.UserName.ToLower().Contains(search)
                );
            }
            // Calculate pagination values
            int totalCount = await query.CountAsync();
            var customers = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResponse<Customer>
            {
                Data = customers,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize),
            };
        }
    }
}
