namespace School.Application.Dtos
{
    public class SubmissionDto
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string? FileUrl { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public decimal? Grade { get; set; }
        public int? GradedByTeacherId { get; set; }
        public string? GradedByTeacherName { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
