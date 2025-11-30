using Microsoft.Extensions.Logging;
using School.Application.Contracts.Persistence;
using School.Application.Contracts.Services;
using School.Application.Dtos;
using School.Application.Requests.Teacher;
using School.Domain.Entities;
using School.Domain.Enums;

namespace School.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public NotificationService(ILogger<NotificationService> logger,
            INotificationRepository notificationRepository,
            IUserRepository userRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<NotificationDto> CreateAsync(CreateNotificationRequest request, int teacherId)
        {
            try
            {
                if (request.RecipientRole.ToLower() != "student")
                {
                    throw new InvalidOperationException("RecipientRole must be 'Student'");
                }

                if (request.RecipientId.HasValue)
                {
                    var recipient = await _userRepository.GetByIdAsync(request.RecipientId.Value);
                    if (recipient is null)
                    {
                        throw new InvalidOperationException("Recipient not found");
                    }

                    if (recipient.Role.ToLower() != "student")
                    {
                        throw new InvalidOperationException("RecipientId must be a Student User Id");
                    }
                }

                var teacher = await _userRepository.GetByIdAsync(teacherId);
                if (teacher is null)
                {
                    throw new InvalidOperationException("Teacher not found");
                }

                Notification notification = new()
                {
                    Title = request.Title,
                    Message = request.Message,
                    RecipientRole = request.RecipientRole,
                    RecipientId = request.RecipientId,
                    CreatedByTeacherId = teacherId,
                    IsRead = false,
                    CreatedDate = DateTime.UtcNow
                };

                var createdNotification = await _notificationRepository.AddAsync(notification);

                NotificationDto notificationDto = new()
                {
                    Id = createdNotification.Id,
                    Title = createdNotification.Title,
                    Message = createdNotification.Message,
                    RecipientRole = createdNotification.RecipientRole,
                    RecipientId = createdNotification.RecipientId,
                    RecipientName = createdNotification.Recipient?.Name,
                    IsRead = createdNotification.IsRead,
                    CreatedByTeacherId = createdNotification.CreatedByTeacherId,
                    CreatedByTeacherName = createdNotification.CreatedByTeacher?.Name,
                    CreatedDate = createdNotification.CreatedDate
                };

                return notificationDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while creating a notification. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<NotificationDto>> GetByTeacherIdAsync(int teacherId)
        {
            try
            {
                var notifications = await _notificationRepository.GetByTeacherIdAsync(teacherId);

                List<NotificationDto> notificationDtos = notifications.Select(notification => new NotificationDto
                {
                    Id = notification.Id,
                    Title = notification.Title,
                    Message = notification.Message,
                    RecipientRole = notification.RecipientRole,
                    RecipientId = notification.RecipientId,
                    RecipientName = notification.Recipient?.Name,
                    IsRead = notification.IsRead,
                    CreatedByTeacherId = notification.CreatedByTeacherId,
                    CreatedByTeacherName = notification.CreatedByTeacher?.Name,
                    CreatedDate = notification.CreatedDate
                }).ToList();

                return notificationDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching notifications for teacher {teacherId}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<NotificationDto>> GetByStudentIdAsync(int studentId)
        {
            try
            {
                var student = await _userRepository.GetByIdAsync(studentId);
                if (student is null)
                {
                    throw new InvalidOperationException("Student not found");
                }

                if (student.Role.ToLower() != "student")
                {
                    throw new InvalidOperationException("Only students can view notifications");
                }

                var notifications = await _notificationRepository.GetByStudentIdAsync(studentId);

                List<NotificationDto> notificationDtos = notifications.Select(notification => new NotificationDto
                {
                    Id = notification.Id,
                    Title = notification.Title,
                    Message = notification.Message,
                    RecipientRole = notification.RecipientRole,
                    RecipientId = notification.RecipientId,
                    RecipientName = notification.Recipient?.Name,
                    IsRead = notification.IsRead,
                    CreatedByTeacherId = notification.CreatedByTeacherId,
                    CreatedByTeacherName = notification.CreatedByTeacher?.Name,
                    CreatedDate = notification.CreatedDate
                }).ToList();

                return notificationDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching notifications for student {studentId}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<NotificationDto> GetByIdAndMarkAsReadAsync(int id, int studentId)
        {
            try
            {
                var student = await _userRepository.GetByIdAsync(studentId);
                if (student is null)
                {
                    throw new InvalidOperationException("Student not found");
                }

                if (student.Role.ToLower() != "student")
                {
                    throw new InvalidOperationException("Only students can read notifications");
                }

                var notification = await _notificationRepository.GetByIdAsync(id);
                if (notification is null)
                {
                    throw new InvalidOperationException("Notification not found");
                }

                if (notification.RecipientRole.ToLower() != "student")
                {
                    throw new InvalidOperationException("Notification is not for students");
                }

                if (notification.RecipientId.HasValue && notification.RecipientId.Value != studentId)
                {
                    throw new InvalidOperationException("You do not have permission to read this notification");
                }

                // Only mark as read if RecipientId matches the logged-in student
                if (notification.RecipientId.HasValue && notification.RecipientId.Value == studentId)
                {
                    notification.IsRead = true;
                    await _notificationRepository.UpdateAsync(notification);
                }

                NotificationDto notificationDto = new()
                {
                    Id = notification.Id,
                    Title = notification.Title,
                    Message = notification.Message,
                    RecipientRole = notification.RecipientRole,
                    RecipientId = notification.RecipientId,
                    RecipientName = notification.Recipient?.Name,
                    IsRead = notification.IsRead,
                    CreatedByTeacherId = notification.CreatedByTeacherId,
                    CreatedByTeacherName = notification.CreatedByTeacher?.Name,
                    CreatedDate = notification.CreatedDate
                };

                return notificationDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while reading notification {id} for student {studentId}. {ex.Message}", ex);
                throw;
            }
        }
    }
}
