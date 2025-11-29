namespace School.Application.Dtos
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string? ClassName { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? StudentEmail { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public int MarkedByTeacherId { get; set; }
        public string? MarkedByTeacherName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
