using School.Application.Dtos.Configuration;

namespace School.Api.Configuration
{
    public static class EmailSetup
    {
        public static IServiceCollection AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            return services;
        }
    }
}

