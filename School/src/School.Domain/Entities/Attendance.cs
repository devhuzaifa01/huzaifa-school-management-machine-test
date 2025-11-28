using School.Domain.Common;
using School.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace School.Domain.Entities
{
    public class Attendance : BaseEntity
    {
        [Required(ErrorMessage = "ClassId is required")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "StudentId is required")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public AttendanceStatus Status { get; set; }

        [Required(ErrorMessage = "MarkedByTeacherId is required")]
        public int MarkedByTeacherId { get; set; }

        public Class? Class { get; set; }
        public User? Student { get; set; }
        public User? MarkedByTeacher { get; set; }
    }
}
