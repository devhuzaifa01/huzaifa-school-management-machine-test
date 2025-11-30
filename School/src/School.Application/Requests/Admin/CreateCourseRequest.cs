namespace School.Application.Requests.Admin
{
    public class CreateCourseRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DepartmentId { get; set; }
        public int Credits { get; set; }
    }
}
