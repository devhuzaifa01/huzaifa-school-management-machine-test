using School.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace School.Domain.Entities
{
    public class Class : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "CourseId is required")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "TeacherId is required")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Semester is required")]
        [MaxLength(50, ErrorMessage = "Semester cannot exceed 50 characters")]
        public string Semester { get; set; } = string.Empty;

        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "IsActive is required")]
        public bool IsActive { get; set; } = true;

        public Course? Course { get; set; }
        public User? Teacher { get; set; }
    }
}
