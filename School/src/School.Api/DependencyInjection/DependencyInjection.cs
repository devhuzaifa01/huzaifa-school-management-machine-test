using School.Api.Filters;
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
            services.AddScoped<ValidationFilter>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<FileUploadService>();

            // Register Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<ISubmissionRepository, SubmissionRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            return services;
        }
    }
}
