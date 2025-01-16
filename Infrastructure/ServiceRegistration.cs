using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Infrastructure.Services.Api;
using Infrastructure.Services.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {

            services.AddHttpClient<IApiService, ApiService>(client =>
            {
                client.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
                client.DefaultRequestHeaders.Add("User-Agent", "Dart/3.4 (dart:io)");
            });
            services.AddScoped<ITokenGeneratorService, TokenGenerator>();
        }
    }
}
