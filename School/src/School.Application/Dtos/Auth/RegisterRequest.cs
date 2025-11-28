using System.ComponentModel.DataAnnotations;

namespace School.Application.Dtos.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("Admin|Teacher|Student", ErrorMessage = "Role must be either Admin, Teacher, or Student")]
        public string Role { get; set; } = string.Empty;
    }
}
