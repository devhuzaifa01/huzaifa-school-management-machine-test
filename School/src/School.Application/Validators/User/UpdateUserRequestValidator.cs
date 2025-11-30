using FluentValidation;
using School.Application.Requests.User;

namespace School.Application.Validators.User
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(role => role == "Admin" || role == "Teacher" || role == "Student")
                .WithMessage("Role must be either Admin, Teacher, or Student")
                .MaximumLength(50).WithMessage("Role cannot exceed 50 characters");
        }
    }
}

