using FluentValidation;
using School.Application.Requests.Course;

namespace School.Application.Validators.Course
{
    public class UpdateCourseRequestValidator : AbstractValidator<UpdateCourseRequest>
    {
        public UpdateCourseRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required")
                .MaximumLength(20).WithMessage("Code cannot exceed 20 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("DepartmentId is required");

            RuleFor(x => x.Credits)
                .InclusiveBetween(1, 10).WithMessage("Credits must be between 1 and 10");
        }
    }
}

