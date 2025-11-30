using School.Application.Dtos;

namespace School.Api.Configuration
{
    public static class FileUploadSetup
    {
        public static IServiceCollection AddFileUploadConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileUploadSettings>(configuration.GetSection("FileUploadSettings"));
            return services;
        }
    }
}

