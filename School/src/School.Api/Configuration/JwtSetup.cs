using School.Application.Dtos.Auth;

namespace School.Api.Configuration
{
    public static class JwtSetup
    {
        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            return services;
        }
    }
}
