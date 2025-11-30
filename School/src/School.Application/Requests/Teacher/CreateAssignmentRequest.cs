namespace School.Application.Requests.Teacher
{
    public class CreateAssignmentRequest
    {
        public int ClassId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}

