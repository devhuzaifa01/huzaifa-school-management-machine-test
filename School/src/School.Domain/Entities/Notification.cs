using School.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace School.Domain.Entities
{
    public class Notification : BaseEntity
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Message is required")]
        [MaxLength(1000, ErrorMessage = "Message cannot exceed 1000 characters")]
        public string Message { get; set; } = string.Empty;

        [Required(ErrorMessage = "RecipientRole is required")]
        [MaxLength(50, ErrorMessage = "RecipientRole cannot exceed 50 characters")]
        public string RecipientRole { get; set; } = string.Empty;
        
        public int? RecipientId { get; set; }

        [Required(ErrorMessage = "IsRead is required")]
        public bool IsRead { get; set; } = false;

        public int CreatedByTeacherId { get; set; }

        public User? Recipient { get; set; }
        public User? CreatedByTeacher { get; set; }
    }
}
