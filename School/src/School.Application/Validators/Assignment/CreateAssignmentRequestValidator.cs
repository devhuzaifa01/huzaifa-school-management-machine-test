using FluentValidation;
using School.Application.Requests.Assignment;

namespace School.Application.Validators.Assignment
{
    public class CreateAssignmentRequestValidator : AbstractValidator<CreateAssignmentRequest>
    {
        public CreateAssignmentRequestValidator()
        {
            RuleFor(x => x.ClassId)
                .GreaterThan(0).WithMessage("ClassId is required");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("DueDate is required");
        }
    }
}

