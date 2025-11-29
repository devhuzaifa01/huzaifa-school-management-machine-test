using System.ComponentModel.DataAnnotations;

namespace School.Application.Requests.Teacher
{
    public class MarkAttendanceRequest
    {
        [Required(ErrorMessage = "ClassId is required")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "StudentId is required")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("Present|Absent|Late", ErrorMessage = "Status must be Present, Absent, or Late")]
        public string Status { get; set; } = string.Empty;
    }
}

