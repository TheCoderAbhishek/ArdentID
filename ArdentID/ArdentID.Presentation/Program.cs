using ArdentID.Presentation.Extensions;
using Serilog;
using System.Reflection;

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

            // Configure Seri log
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
            builder.Host.UseSerilog();

            // --- Add services to the container using Extension Methods ---
            builder.Services
                .AddInfrastructureServices(builder.Configuration)
                .AddApplicationServices()
                .AddPresentationServices();

            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

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