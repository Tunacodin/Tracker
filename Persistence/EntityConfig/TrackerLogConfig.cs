using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.HelperClass;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfig
{
    public class TrackerLogConfig : BaseConfig<TrackerLog>
    {
        public override void Configure(EntityTypeBuilder<TrackerLog> builder)
        {
            base.Configure(builder);
        }
    }
}
