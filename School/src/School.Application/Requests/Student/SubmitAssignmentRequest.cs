namespace School.Application.Requests.Student
{
    public class SubmitAssignmentRequest
    {
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
    }
}

