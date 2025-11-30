using FluentValidation;
using School.Application.Requests.Teacher;

namespace School.Application.Validators.Notification
{
    public class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequest>
    {
        public CreateNotificationRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required")
                .MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters");

            RuleFor(x => x.RecipientRole)
                .NotEmpty().WithMessage("RecipientRole is required")
                .Must(role => role.ToLower() == "student")
                .WithMessage("RecipientRole must be 'Student'");
        }
    }
}

