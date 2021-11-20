using CorewebAPI.Entities.Model;
using CorewebAPI.LoggerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CorewebAPI.Helper
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
        public static void ConfigurMSSQLDBContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:DatabaseConnection"];
            services.AddDbContext<AppDBContext>(o => o.UseSqlServer(connectionString));
        }

        public static void ConfigurAppSettings(this IServiceCollection services, IConfiguration config)
        {
            // configure strongly typed settings object
            services.Configure<AppSettings>(config.GetSection("AppSettings"));
        }
    }
}
