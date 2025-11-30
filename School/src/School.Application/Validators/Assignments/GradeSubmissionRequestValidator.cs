using FluentValidation;
using School.Application.Requests.Teacher;

namespace School.Application.Validators.Assignments
{
    public class GradeSubmissionRequestValidator : AbstractValidator<GradeSubmissionRequest>
    {
        public GradeSubmissionRequestValidator()
        {
            RuleFor(x => x.Grade)
                .NotNull().WithMessage("Grade is required")
                .InclusiveBetween(0, 100).WithMessage("Grade must be between 0 and 100");

            RuleFor(x => x.Remarks)
                .MaximumLength(500).WithMessage("Remarks cannot exceed 500 characters");
        }
    }
}
