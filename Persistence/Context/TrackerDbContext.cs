using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.HelperClass;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityConfig;

namespace Persistence.Context
{
    public class TrackerDbContext : IdentityDbContext<Customer>
    {
        public TrackerDbContext(DbContextOptions<TrackerDbContext> options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Code> Codes { get; set; }
        public DbSet<TrackerLog> TrackerLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new AccountConfig());
            modelBuilder.ApplyConfiguration(new CodeConfig());
            modelBuilder.ApplyConfiguration(new TrackerLogConfig());
            Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            DataGenerator.Initialize(modelBuilder);
        }
    }
}
