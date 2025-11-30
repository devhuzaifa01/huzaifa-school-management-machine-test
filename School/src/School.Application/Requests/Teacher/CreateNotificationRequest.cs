namespace School.Application.Requests.Teacher
{
    public class CreateNotificationRequest
    {
        public int? ClassId { get; set; }
        public List<int>? StudentIds { get; set; }
        public int? RecipientId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}

