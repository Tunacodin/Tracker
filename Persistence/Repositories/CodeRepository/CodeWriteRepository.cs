using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repositories.ICodeRepositories;
using Domain.Entities;
using Persistence.Context;

namespace Persistence.Repositories.CodeRepository
{
    public class CodeWriteRepository : WriteRepository<Code>, ICodeWriteRepository
    {
        public CodeWriteRepository(TrackerDbContext db)
            : base(db) { }
    }
}
