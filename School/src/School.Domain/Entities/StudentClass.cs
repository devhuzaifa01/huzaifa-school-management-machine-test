using School.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace School.Domain.Entities
{
    public class StudentClass : BaseEntity
    {
        [Required(ErrorMessage = "StudentId is required")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "ClassId is required")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "EnrollmentDate is required")]
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        public User? Student { get; set; }
        public Class? Class { get; set; }
    }
}
