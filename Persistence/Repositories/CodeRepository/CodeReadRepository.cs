using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Repositories;
using Application.Repositories.ICodeRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.CodeRepository
{
    public class CodeReadRepository : ReadRepository<Code>, ICodeReadRepository
    {
        public CodeReadRepository(TrackerDbContext db)
            : base(db) { }
    }
}
