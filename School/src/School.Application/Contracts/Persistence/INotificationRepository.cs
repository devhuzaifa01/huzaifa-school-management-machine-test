using School.Domain.Entities;

namespace School.Application.Contracts.Persistence
{
    public interface INotificationRepository
    {
        Task<Notification> AddAsync(Notification notification);
        Task<List<Notification>> AddRangeAsync(List<Notification> notifications);
        Task<List<Notification>> GetByTeacherIdAsync(int teacherId);
        Task<List<Notification>> GetByStudentIdAsync(int studentId);
        Task<Notification?> GetByIdAsync(int id);
        Task<Notification> UpdateAsync(Notification notification);
    }
}
