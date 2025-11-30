namespace School.Application.Requests.Course
{
    public class UpdateCourseRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DepartmentId { get; set; }
        public int Credits { get; set; }
    }
}

