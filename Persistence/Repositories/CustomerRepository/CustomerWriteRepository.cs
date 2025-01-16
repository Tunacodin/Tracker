using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repositories.ICustomerRepositories;
using Domain.Entities;
using Persistence.Context;

namespace Persistence.Repositories.CustomerRepository
{
    public class CustomerWriteRepository : WriteRepository<Customer>, ICustomerWriteRepository
    {
        public CustomerWriteRepository(TrackerDbContext db)
            : base(db) { }
    }
}
