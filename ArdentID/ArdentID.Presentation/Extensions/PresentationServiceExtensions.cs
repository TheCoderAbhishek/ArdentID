using ArdentID.Application.Validators.Authentication;
using ArdentID.Presentation.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ArdentID.Presentation.Extensions
{
    /// <summary>
    /// Contains extension methods for configuring presentation-layer services in the dependency injection container.
    /// </summary>
    public static class PresentationServiceExtensions
    {
        /// <summary>
        /// Configures and registers services specific to the Presentation layer.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configuration">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Register your custom validation filter.
            services.AddScoped<ValidationFilter>();

            // 2. Add controllers and register the filter globally.
            services.AddControllers(options => options.Filters.Add<ValidationFilter>());

            // 3. Scan the Application assembly and register all your validators.
            services.AddValidatorsFromAssemblyContaining<UserRegistrationDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginRequestDtoValidator>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Configure JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!))
                };
            });

            return services;
        }
    }
}
