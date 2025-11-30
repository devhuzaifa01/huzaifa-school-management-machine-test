using FluentValidation;
using School.Application.Requests.Class;

namespace School.Application.Validators.Classes
{
    public class CreateClassRequestValidator : AbstractValidator<CreateClassRequest>
    {
        public CreateClassRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage("CourseId is required");

            RuleFor(x => x.Semester)
                .NotEmpty().WithMessage("Semester is required")
                .MaximumLength(50).WithMessage("Semester cannot exceed 50 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("EndDate is required");
        }
    }
}
