using Application.Repositories.IAccountRepositories;
using Domain.Entities;
using Persistence.Context;

namespace Persistence.Repositories.AccountRepository
{
    public class AccountWriteRepository : WriteRepository<Account>, IAccountWriteRepository
    {
        public AccountWriteRepository(TrackerDbContext db) : base(db)
        {
        }
    }
}
