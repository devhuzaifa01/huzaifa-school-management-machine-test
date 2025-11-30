using FluentValidation;
using School.Application.Requests.Notification;

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

            RuleFor(x => x)
                .Must(HaveAtLeastOneRecipient)
                .WithMessage("Provide either ClassId, StudentIds, or RecipientId");

            When(x => x.StudentIds != null && x.StudentIds.Count > 0, () =>
            {
                RuleForEach(x => x.StudentIds)
                    .GreaterThan(0).WithMessage("Student ID must be greater than 0");
            });
        }

        private bool HaveAtLeastOneRecipient(CreateNotificationRequest request)
        {
            return request.ClassId.HasValue 
                || (request.StudentIds != null && request.StudentIds.Count > 0) 
                || request.RecipientId.HasValue;
        }
    }
}

