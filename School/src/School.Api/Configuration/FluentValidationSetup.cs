using FluentValidation;
using School.Application.Validators;

namespace School.Api.Configuration
{
    public static class FluentValidationSetup
    {
        public static IServiceCollection AddFluentValidations(this IServiceCollection services)
        {
            // This registers all FluentValidations from the application assembly
            services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);

            return services;
        }
    }
}
