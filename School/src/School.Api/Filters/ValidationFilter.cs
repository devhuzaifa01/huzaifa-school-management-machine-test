using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace School.Api.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // FluentValidations
            foreach (var parameter in context.ActionDescriptor.Parameters)
            {
                if (context.ActionArguments.TryGetValue(parameter.Name, out var argument) && argument != null)
                {
                    var argumentType = argument.GetType();
                    var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
                    var validator = _serviceProvider.GetService(validatorType) as IValidator;

                    if (validator != null)
                    {
                        var validationContext = new ValidationContext<object>(argument);
                        var validationResult = await validator.ValidateAsync(validationContext);

                        if (!validationResult.IsValid)
                        {
                            foreach (var error in validationResult.Errors)
                            {
                                context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                            }
                        }
                    }
                }
            }

            // ModelState after FluentValidation
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value!.Errors.Select(e => e.ErrorMessage) });

                context.Result = new BadRequestObjectResult(errors);
                return;
            }

            await next();
        }
    }
}
