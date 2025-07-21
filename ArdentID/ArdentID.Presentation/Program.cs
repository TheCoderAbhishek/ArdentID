using ArdentID.Application.Interfaces;
using ArdentID.Application.Services;
using ArdentID.Infrastructure.Persistence.Data;
using ArdentID.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ArdentID.Presentation
{
    /// <summary>
    /// The main entry point class for the application.
    /// This class is responsible for configuring and running the ASP.NET Core web application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the web application.
        /// This method sets up the web host, configures services, and defines the HTTP request pipeline.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application. These are used to configure the web host.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Register the DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString,
                    // This is CRUCIAL for migrations in a separate project
                    b => b.MigrationsAssembly("ArdentID.Infrastructure")));

            // Add services to the container.
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(builder.Configuration)
                                .CreateLogger();

            builder.Host.UseSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}