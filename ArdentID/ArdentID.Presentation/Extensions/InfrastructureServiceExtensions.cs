using ArdentID.Application.Interfaces.Authentication;
using ArdentID.Application.Interfaces.Shared;
using ArdentID.Application.Interfaces.UserManagement;
using ArdentID.Application.Services;
using ArdentID.Infrastructure.Persistence.Data;
using ArdentID.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArdentID.Presentation.Extensions
{
    /// <summary>
    /// Contains extension methods for configuring infrastructure-layer services in the dependency injection container.
    /// </summary>
    public static class InfrastructureServiceExtensions
    {
        /// <summary>
        /// Registers services from the Infrastructure layer with the dependency injection container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configuration">The application's <see cref="IConfiguration"/> instance, used to retrieve settings like connection strings.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Register the DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly("ArdentID.Infrastructure")));

            // Register Repositories
            services.AddScoped<IUserRepository, UserRepository>();

            // Register Security & Other Services
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IDataProtectionService, DataProtectionService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            // This is required for the Data Protection API to work
            services.AddDataProtection();

            return services;
        }
    }
}
