using Microsoft.EntityFrameworkCore;
using School.Application.Contracts.Persistence;
using School.Domain.Entities;
using School.Infrastructure.Persistence;

namespace School.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly SchoolDbContext _dbContext;

        public NotificationRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Notification> AddAsync(Notification notification)
        {
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();
            return notification;
        }

        public async Task<List<Notification>> AddRangeAsync(List<Notification> notifications)
        {
            _dbContext.Notifications.AddRange(notifications);
            await _dbContext.SaveChangesAsync();
            return notifications;
        }

        public async Task<List<Notification>> GetByTeacherIdAsync(int teacherId)
        {
            return await _dbContext.Notifications
                .Include(n => n.Recipient)
                .Include(n => n.CreatedByTeacher)
                .Where(n => n.CreatedByTeacherId == teacherId && (n.IsDeleted == null || n.IsDeleted == false))
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetByStudentIdAsync(int studentId)
        {
            return await _dbContext.Notifications
                .Include(n => n.Recipient)
                .Include(n => n.CreatedByTeacher)
                .Where(n => n.RecipientId == studentId
                    && (n.IsDeleted == null || n.IsDeleted == false))
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _dbContext.Notifications
                .Include(n => n.Recipient)
                .Include(n => n.CreatedByTeacher)
                .FirstOrDefaultAsync(n => n.Id == id && (n.IsDeleted == null || n.IsDeleted == false));
        }

        public async Task<Notification> UpdateAsync(Notification notification)
        {
            _dbContext.Notifications.Update(notification);
            await _dbContext.SaveChangesAsync();
            return notification;
        }
    }
}
