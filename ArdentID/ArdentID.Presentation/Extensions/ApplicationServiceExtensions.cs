using ArdentID.Application.Interfaces.Authentication;
using ArdentID.Application.Services;

namespace ArdentID.Presentation.Extensions
{
    /// <summary>
    /// Contains extension methods for configuring application-layer services in the dependency injection container.
    /// </summary>
    public static class ApplicationServiceExtensions
    {
        /// <summary>
        /// Registers services from the Application layer with the dependency injection container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register all your application-layer services here
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}
