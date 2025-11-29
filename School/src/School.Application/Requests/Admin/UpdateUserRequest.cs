using System.ComponentModel.DataAnnotations;

namespace School.Application.Requests.Admin
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("Admin|Teacher|Student", ErrorMessage = "Role must be either Admin, Teacher, or Student")]
        [MaxLength(50, ErrorMessage = "Role cannot exceed 50 characters")]
        public string Role { get; set; } = string.Empty;
    }
}

