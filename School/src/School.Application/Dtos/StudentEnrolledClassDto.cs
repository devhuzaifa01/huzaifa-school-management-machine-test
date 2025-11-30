namespace School.Application.Dtos
{
    public class StudentEnrolledClassDto
    {
        // Enrollment details
        public int EnrollmentId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime EnrollmentCreatedDate { get; set; }
        public DateTime? EnrollmentUpdatedDate { get; set; }

        // Class details
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        // Course details
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? CourseCode { get; set; }

        // Teacher details
        public int TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string? TeacherEmail { get; set; }
    }
}

