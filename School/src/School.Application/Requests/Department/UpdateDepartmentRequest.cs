namespace School.Application.Requests.Department
{
    public class UpdateDepartmentRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? HeadOfDepartmentId { get; set; }
    }
}

