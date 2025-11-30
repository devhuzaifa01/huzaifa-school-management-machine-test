using School.Application.Dtos;
using School.Application.Requests.Notification;

namespace School.Application.Contracts.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(CreateNotificationRequest request, int teacherId);
        Task<List<NotificationDto>> GetByTeacherIdAsync(int teacherId);
        Task<List<NotificationDto>> GetByStudentIdAsync(int studentId);
        Task<NotificationDto> GetByIdAndMarkAsReadAsync(int id, int studentId);
    }
}
