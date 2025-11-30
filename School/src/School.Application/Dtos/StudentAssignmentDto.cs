namespace School.Application.Dtos
{
    public class StudentAssignmentDto
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string? ClassName { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public int CreatedByTeacherId { get; set; }
        public string? CreatedByTeacherName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}

