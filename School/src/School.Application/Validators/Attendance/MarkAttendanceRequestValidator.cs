using FluentValidation;
using School.Application.Requests.Teacher;

namespace School.Application.Validators.Attendance
{
    public class MarkAttendanceRequestValidator : AbstractValidator<MarkAttendanceRequest>
    {
        public MarkAttendanceRequestValidator()
        {
            RuleFor(x => x.ClassId)
                .GreaterThan(0).WithMessage("ClassId is required");

            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("StudentId is required");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .Must(status => status == "Present" || status == "Absent" || status == "Late")
                .WithMessage("Status must be Present, Absent, or Late");
        }
    }
}
