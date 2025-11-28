using School.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace School.Domain.Entities
{
    public class Assignment : BaseEntity
    {
        [Required(ErrorMessage = "ClassId is required")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "DueDate is required")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "CreatedByTeacherId is required")]
        public int CreatedByTeacherId { get; set; }

        public Class? Class { get; set; }
        public User? CreatedByTeacher { get; set; }
    }
}
