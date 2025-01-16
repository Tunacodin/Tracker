using System.Reflection;
using Application.Repositories.IAccountRepositories;
using Application.Repositories.ICodeRepositories;
using Application.Repositories.ICustomerRepositories;
using Application.Repositories.ILoggerReadRepositories;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.ConcreteServices.AccountService;
using Persistence.ConcreteServices.CustomerService;
using Persistence.ConcreteServices.Logger;
using Persistence.Context;
using Persistence.Mapping;
using Persistence.Repositories.AccountRepository;
using Persistence.Repositories.CodeRepository;
using Persistence.Repositories.CustomerRepository;
using Persistence.Repositories.LoggerRepository;

namespace Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<TrackerDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TrackerDb"))
            );
            services
                .AddIdentity<Customer, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TrackerDbContext>()
                .AddDefaultTokenProviders();
            //Repositories
            services.AddScoped<ILoggerReadRepository, LoggerReadRepository>();
            services.AddScoped<ILoggerWriteRepository, LoggerWriteRepository>();
            services.AddScoped<IAccountReadRepository, AccountReadRepository>();
            services.AddScoped<IAccountWriteRepository, AccountWriteRepository>();
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            services.AddScoped<ICodeReadRepository, CodeReadRepository>();
            services.AddScoped<ICodeWriteRepository, CodeWriteRepository>();
            //Services
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ILoggerService, LoggerService>();
            //Mapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
