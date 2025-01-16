using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repositories.ILoggerReadRepositories;
using Domain.HelperClass;
using Persistence.Context;

namespace Persistence.Repositories.LoggerRepository
{
    public class LoggerReadRepository : ReadRepository<TrackerLog>, ILoggerReadRepository
    {
        public LoggerReadRepository(TrackerDbContext db)
            : base(db) { }
    }
}
