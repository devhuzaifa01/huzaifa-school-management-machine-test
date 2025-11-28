using Microsoft.EntityFrameworkCore;
using School.Infrastructure.Persistence;
namespace School.Api.Configuration
{
    public static class DatabaseSetup
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<SchoolDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
