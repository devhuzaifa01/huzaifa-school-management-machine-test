using FluentValidation;
using School.Application.Requests.Admin;

namespace School.Application.Validators.Department
{
    public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
    {
        public CreateDepartmentRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}

