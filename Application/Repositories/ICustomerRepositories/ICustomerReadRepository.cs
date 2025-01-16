using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Utilities.Response;
using Domain.Entities;

namespace Application.Repositories.ICustomerRepositories
{
    public interface ICustomerReadRepository : IReadRepository<Customer>
    {
        Task<Customer> GetCustomer(string id);
        Task<Customer> GetCustomerByEmail(string email);
        Task<PaginatedResponse<Customer>> SearchCustomerPaginated(
            string search,
            PaginationDTO pagination
        );
    }
}
