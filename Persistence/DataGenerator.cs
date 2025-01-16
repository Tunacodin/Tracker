using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utilities.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;

namespace Persistence
{
    public class DataGenerator
    {
        public static void Initialize(ModelBuilder modelBuilder)
        {
            var masterRoleId = Guid.NewGuid().ToString();
            var customerRoleId = Guid.NewGuid().ToString();
            var adminUserId = Guid.NewGuid().ToString();
            //{
            //    "fullName": "admin",
            //  "email": "test@test.com",
            //  "userName": "admin",
            //  "password": "Admin.123",
            //  "passwordConfirm": "Admin.123"
            //}
            modelBuilder
                .Entity<IdentityRole>()
                .HasData(
                    new IdentityRole
                    {
                        Id = masterRoleId,
                        Name = Roles.Master,
                        NormalizedName = Roles.Master.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    },
                    new IdentityRole
                    {
                        Id = customerRoleId,
                        Name = Roles.Customer,
                        NormalizedName = Roles.Customer.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    }
                );
            modelBuilder
                .Entity<Customer>()
                .HasData(
                    new Customer
                    {
                        UserName = "admin",
                        FullName = "admin",
                        Id = adminUserId,
                        Email = "test@test.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "TEST@TEST.COM",
                        NormalizedUserName = "ADMIN",
                        PasswordHash =
                            "AQAAAAEAACcQAAAAEMIvRA61NHINPY1pfBNT5SXrLSS5VKO2YnCF4z2oMTJR/Gu2PLxFRVMjtX39I4apwg==",
                        CreationDate = DateTime.UtcNow,
                    }
                );

            // Add user-role relationship
            modelBuilder
                .Entity<IdentityUserRole<string>>()
                .HasData(
                    new IdentityUserRole<string> { RoleId = masterRoleId, UserId = adminUserId }
                );
        }
    }
}
