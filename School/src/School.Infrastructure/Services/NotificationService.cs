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
        private readonly IClassRepository _classRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public NotificationService(ILogger<NotificationService> logger,
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IClassRepository classRepository,
            IEnrollmentRepository enrollmentRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _classRepository = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
            _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
        }

        public async Task SendNotificationAsync(CreateNotificationRequest request, int teacherId)
        {
            try
            {
                var teacher = await _userRepository.GetByIdAsync(teacherId);
                if (teacher is null)
                {
                    throw new InvalidOperationException("Teacher not found");
                }

                List<int> studentIds = new();

                // Case 1: If ClassId is provided
                // Fetch all students for this Class
                if (request.ClassId.HasValue)
                {
                    var classEntity = await _classRepository.GetByIdAsync(request.ClassId.Value);
                    if (classEntity is null)
                    {
                        throw new InvalidOperationException("Class not found");
                    }

                    var enrollments = await _enrollmentRepository.GetByClassIdAsync(request.ClassId.Value);
                    studentIds = enrollments.Select(e => e.StudentId).ToList();

                    if (studentIds.Count == 0)
                    {
                        throw new InvalidOperationException("No students enrolled in this class");
                    }
                }
                // Case 2: If StudentIds list is provided
                // Send to specified students
                else if (request.StudentIds != null && request.StudentIds.Count > 0)
                {
                    studentIds = request.StudentIds;
                }
                // Case 3: If Only RecipientId is provided
                // Single Recipient
                else if (request.RecipientId.HasValue)
                {
                    studentIds.Add(request.RecipientId.Value);
                }
                else
                {
                    throw new InvalidOperationException("Invalid notification request. Provide either ClassId, StudentIds, or RecipientId");
                }

                foreach (var studentId in studentIds)
                {
                    var student = await _userRepository.GetByIdAsync(studentId);
                    if (student is null)
                    {
                        throw new InvalidOperationException($"Student with ID {studentId} not found");
                    }

                    if (student.Role.ToLower() != "student")
                    {
                        throw new InvalidOperationException($"User with ID {studentId} is not a student");
                    }
                }

                List<Notification> notifications = studentIds.Select(studentId => new Notification
                {
                    Title = request.Title,
                    Message = request.Message,
                    RecipientRole = "Student",
                    RecipientId = studentId,
                    CreatedByTeacherId = teacherId,
                    IsRead = false,
                    CreatedDate = DateTime.UtcNow
                }).ToList();

                await _notificationRepository.AddRangeAsync(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while sending notifications. {ex.Message}", ex);
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

                if (notification.RecipientId != studentId)
                {
                    throw new InvalidOperationException("You do not have permission to read this notification");
                }

                notification.IsRead = true;
                await _notificationRepository.UpdateAsync(notification);

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
