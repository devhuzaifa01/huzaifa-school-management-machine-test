namespace School.Application.Requests.Teacher
{
    public class CreateNotificationRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string RecipientRole { get; set; } = string.Empty;
        public int? RecipientId { get; set; }
    }
}

