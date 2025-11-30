using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using School.Application.Common.Errors;

namespace School.Api.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            _logger.LogError(exception, "Unhandled exception occurred");

            if (exception is NotFoundException notFoundEx)
            {
                context.Result = new JsonResult(new
                {
                    success = false,
                    message = notFoundEx.Message
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };

                context.ExceptionHandled = true;
                return;
            }

            if (exception is BusinessException businessEx)
            {
                context.Result = new JsonResult(new
                {
                    success = false,
                    message = businessEx.Message
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };

                context.ExceptionHandled = true;
                return;
            }

            if (exception is UnauthorizedException unauthorizedEx)
            {
                context.Result = new JsonResult(new
                {
                    success = false,
                    message = unauthorizedEx.Message
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };

                context.ExceptionHandled = true;
                return;
            }

            if (exception is ValidationException validationEx)
            {
                context.Result = new JsonResult(new
                {
                    success = false,
                    errors = validationEx.Errors
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };

                context.ExceptionHandled = true;
                return;
            }

            context.Result = new JsonResult(new
            {
                success = false,
                message = "An unexpected error occurred."
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
