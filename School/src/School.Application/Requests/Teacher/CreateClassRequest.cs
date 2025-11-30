namespace School.Application.Requests.Teacher
{
    public class CreateClassRequest
    {
        public string Name { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string Semester { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

