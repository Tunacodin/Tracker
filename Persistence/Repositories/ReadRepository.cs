using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Repositories;
using Application.Utilities.Response;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T>
        where T : class, IBaseEntity
    {
        private readonly TrackerDbContext db;

        public ReadRepository(TrackerDbContext db)
        {
            this.db = db;
        }

        public DbSet<T> Table => db.Set<T>();

        public IQueryable<T> GetAll(bool asNoTracking = false)
        {
            var query = asNoTracking ? Table.AsNoTracking() : Table;
            return query
                .Where(a => a.Status != Domain.Enums.Status.Pasive)
                .OrderByDescending(a => a.CreationDate);
        }

        public async Task<T> GetByIdAsync(string id) =>
            await Table
                .Where(a => a.Id == id && a.Status != Domain.Enums.Status.Pasive)
                .FirstOrDefaultAsync();

        public async Task<PaginatedResponse<T>> GetPaginatedAsync(
            Expression<Func<T, bool>> predicate,
            PaginationDTO pagination,
            bool tracking = true
        )
        {
            var query = tracking ? Table.AsTracking() : Table.AsNoTracking();

            // Apply the predicate filter
            query = query.Where(predicate);

            // Get total count for pagination
            var totalCount = await query.CountAsync();

            // Calculate total pages
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

            // Get paginated data
            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            // Create paginated response
            var paginatedResponse = new PaginatedResponse<T>
            {
                Data = items,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = totalPages,
                TotalCount = totalCount,
            };

            return paginatedResponse;
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression) =>
            await Table
                .Where(a => a.Status != Domain.Enums.Status.Pasive)
                .FirstOrDefaultAsync(expression);

        public IQueryable<T> GetWhere(
            Expression<Func<T, bool>> expression,
            bool asNoTracking = false
        )
        {
            var query = asNoTracking ? Table.AsNoTracking() : Table;
            return query
                .Where(a => a.Status != Domain.Enums.Status.Pasive)
                .Where(expression)
                .OrderByDescending(a => a.CreationDate);
        }
    }
}
