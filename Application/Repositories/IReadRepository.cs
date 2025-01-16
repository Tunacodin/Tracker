using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Utilities.Response;
using Domain.Interfaces;

namespace Application.Repositories
{
    public interface IReadRepository<T> : IRepository<T>
        where T : class, IBaseEntity
    {
        IQueryable<T> GetAll(bool asNoTracking = false);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, bool asNoTracking = false);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(string id);
        Task<PaginatedResponse<T>> GetPaginatedAsync(
            Expression<Func<T, bool>> predicate,
            PaginationDTO pagination,
            bool tracking = true
        );
    }
}
