using School.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace School.Domain.Entities
{
    public class Submission : BaseEntity
    {
        [Required(ErrorMessage = "AssignmentId is required")]
        public int AssignmentId { get; set; }

        [Required(ErrorMessage = "StudentId is required")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "SubmittedDate is required")]
        public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(500, ErrorMessage = "FileUrl cannot exceed 500 characters")]
        public string? FileUrl { get; set; }

        [Required(ErrorMessage = "Original file name is required")]
        [MaxLength(255, ErrorMessage = "Original file name cannot exceed 255 characters")]
        public string OriginalFileName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Stored file name is required")]
        [MaxLength(255, ErrorMessage = "Stored file name cannot exceed 255 characters")]
        public string StoredFileName { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        public decimal? Grade { get; set; }

        public int? GradedByTeacherId { get; set; }

        [MaxLength(500, ErrorMessage = "Remarks cannot exceed 500 characters")]
        public string? Remarks { get; set; }

        public Assignment? Assignment { get; set; }
        public User? Student { get; set; }
        public User? GradedByTeacher { get; set; }
    }
}
