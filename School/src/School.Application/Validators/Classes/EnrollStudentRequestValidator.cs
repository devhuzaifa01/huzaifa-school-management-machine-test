using FluentValidation;
using School.Application.Requests.Teacher;

namespace School.Application.Validators.Classes
{
    public class EnrollStudentRequestValidator : AbstractValidator<EnrollStudentRequest>
    {
        public EnrollStudentRequestValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("StudentId is required");
        }
    }
}
