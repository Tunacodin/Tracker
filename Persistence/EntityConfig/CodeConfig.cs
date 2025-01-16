using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfig
{
    public class CodeConfig : BaseConfig<Code>
    {
        public override void Configure(EntityTypeBuilder<Code> builder)
        {
            builder.Property(a => a.CustomerId).IsRequired();
            builder
                .Property(a => a.AccountPhoneNumbers)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                );
            base.Configure(builder);
        }
    }
}
