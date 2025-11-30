namespace School.Application.Requests.Admin
{
    public class CreateDepartmentRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? HeadOfDepartmentId { get; set; }
    }
}
