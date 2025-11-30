using FluentValidation;
using School.Application.Dtos.Auth;
using System.Text.RegularExpressions;

namespace School.Application.Validators.Auth
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters")
                .Must(BeAValidEmail).WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Must(BeAValidPassword).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(role => role == "Admin" || role == "Teacher" || role == "Student")
                .WithMessage("Role must be either Admin, Teacher, or Student");
        }

        private bool BeAValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Strict email validation: requires proper format with domain and TLD
            // Pattern: local-part@domain.tld
            // - Local part: letters, numbers, dots, underscores, percent, plus, hyphens
            // - @ symbol
            // - Domain: letters, numbers, dots, hyphens
            // - Dot
            // - TLD: at least 2 letters
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool BeAValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // At least one uppercase letter
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return false;

            // At least one lowercase letter
            if (!Regex.IsMatch(password, @"[a-z]"))
                return false;

            // At least one number
            if (!Regex.IsMatch(password, @"[0-9]"))
                return false;

            // At least one special character
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
                return false;

            return true;
        }
    }
}

