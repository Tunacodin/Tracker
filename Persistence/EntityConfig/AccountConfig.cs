using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfig
{
    public class AccountConfig : BaseConfig<Account>
    {
        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(a => a.Phone).HasMaxLength(50).IsRequired();
            builder.Property(a => a.Password).HasMaxLength(250).IsRequired();
            builder
                .Property(a => a.NeedToVerify)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                );
            base.Configure(builder);
        }
    }
}
