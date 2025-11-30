using System.ComponentModel.DataAnnotations;

namespace School.Application.Requests.Teacher
{
    public class GradeSubmissionRequest
    {
        [Required(ErrorMessage = "Grade is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        public decimal Grade { get; set; }

        [MaxLength(500, ErrorMessage = "Remarks cannot exceed 500 characters")]
        public string? Remarks { get; set; }
    }
}
