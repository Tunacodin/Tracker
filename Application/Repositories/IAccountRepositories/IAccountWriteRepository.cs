using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.IAccountRepositories
{
    public interface IAccountWriteRepository : IWriteRepository<Account>
    {
    }
}
