namespace School.Application.Requests.Teacher
{
    public class UpdateClassRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string Semester { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}

