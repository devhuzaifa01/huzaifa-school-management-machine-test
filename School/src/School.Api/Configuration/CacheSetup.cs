using School.Application.Dtos.Configuration;

namespace School.Api.Configuration
{
    public static class CacheSetup
    {
        public static IServiceCollection AddCacheConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheSettings>(configuration.GetSection("Caching"));
            return services;
        }
    }
}

