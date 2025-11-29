using System.ComponentModel.DataAnnotations;

namespace School.Application.Requests.Teacher
{
    public class EnrollStudentRequest
    {
        [Required(ErrorMessage = "StudentId is required")]
        public int StudentId { get; set; }
    }
}
