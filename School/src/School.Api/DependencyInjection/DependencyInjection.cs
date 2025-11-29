using School.Application.Contracts.Persistence;
using School.Application.Contracts.Services;
using School.Infrastructure.Identity;
using School.Infrastructure.Persistence.Repositories;
using School.Infrastructure.Services;

namespace School.Api.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Services
            services.AddScoped<JwtTokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IUserService, UserService>();

            // Register Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();

            return services;
        }
    }
}
