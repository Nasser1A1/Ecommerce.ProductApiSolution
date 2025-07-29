using eCommerce.SharedLib.DependencyIncjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(
            this IServiceCollection services, 
            IConfiguration configuration) 
        {
            // Add Database Context
            // and Repositories to the service collection
            // Add authentication and authorization services
            // and Serilog configuration
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, configuration, configuration["MySerilog:FileName"]!);

            // Dependency Injection for Repositories
            services.AddScoped<IProduct, ProductRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            //Register shared policies (MiddleWares)
            // such as JWT authentication, Global Exception handling, etc.
            // Listen to only Api Gateway Middleware
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
