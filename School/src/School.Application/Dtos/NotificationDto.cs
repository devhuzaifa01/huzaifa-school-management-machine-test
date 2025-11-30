namespace School.Application.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string RecipientRole { get; set; } = string.Empty;
        public int? RecipientId { get; set; }
        public string? RecipientName { get; set; }
        public bool IsRead { get; set; }
        public int CreatedByTeacherId { get; set; }
        public string? CreatedByTeacherName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
