using Microsoft.Extensions.Logging;
using School.Application.Contracts.Persistence;
using School.Application.Contracts.Services;
using School.Application.Dtos;
using School.Application.Requests.Teacher;
using School.Domain.Entities;
using School.Domain.Enums;

namespace School.Infrastructure.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ILogger<EnrollmentService> _logger;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IClassRepository _classRepository;
        private readonly IUserRepository _userRepository;

        public EnrollmentService(ILogger<EnrollmentService> logger,
            IEnrollmentRepository enrollmentRepository,
            IClassRepository classRepository,
            IUserRepository userRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
            _classRepository = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<StudentClassDto> EnrollStudentAsync(int classId, EnrollStudentRequest request, int teacherId)
        {
            try
            {
                var classEntity = await _classRepository.GetByIdAndTeacherIdAsync(classId, teacherId);
                if (classEntity is null)
                {
                    throw new InvalidOperationException("Class not found or you do not have permission to enroll students in this class");
                }

                if (!classEntity.IsActive)
                {
                    throw new InvalidOperationException("Cannot enroll students in a deactivated class");
                }

                var student = await _userRepository.GetByIdAsync(request.StudentId);
                if (student is null)
                {
                    throw new InvalidOperationException("Student not found");
                }

                if (student.Role != UserRole.Student.ToString())
                {
                    throw new InvalidOperationException("Only students can be enrolled in a class");
                }

                var existingEnrollment = await _enrollmentRepository.GetByStudentIdAndClassIdAsync(request.StudentId, classId);
                if (existingEnrollment is not null)
                {
                    throw new InvalidOperationException("Student is already enrolled in this class");
                }

                StudentClass studentClass = new()
                {
                    StudentId = request.StudentId,
                    ClassId = classId,
                    EnrollmentDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow
                };

                await _enrollmentRepository.AddAsync(studentClass);

                var createdEnrollment = await _enrollmentRepository.GetByIdAsync(studentClass.Id);

                StudentClassDto studentClassDto = new()
                {
                    Id = createdEnrollment!.Id,
                    StudentId = createdEnrollment.StudentId,
                    StudentName = createdEnrollment.Student?.Name,
                    StudentEmail = createdEnrollment.Student?.Email,
                    ClassId = createdEnrollment.ClassId,
                    ClassName = createdEnrollment.Class?.Name,
                    EnrollmentDate = createdEnrollment.EnrollmentDate,
                    CreatedDate = createdEnrollment.CreatedDate,
                    UpdatedDate = createdEnrollment.UpdatedDate
                };

                return studentClassDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while enrolling student {request.StudentId} in class {classId}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<StudentClassDto>> GetEnrollmentsByClassIdAsync(int classId, int teacherId)
        {
            try
            {
                var classEntity = await _classRepository.GetByIdAndTeacherIdAsync(classId, teacherId);
                if (classEntity is null)
                {
                    throw new InvalidOperationException("Class not found or you do not have permission to view enrollments for this class");
                }

                var enrollments = await _enrollmentRepository.GetByClassIdAsync(classId);

                List<StudentClassDto> enrollmentDtos = enrollments.Select(enrollment => new StudentClassDto
                {
                    Id = enrollment.Id,
                    StudentId = enrollment.StudentId,
                    StudentName = enrollment.Student?.Name,
                    StudentEmail = enrollment.Student?.Email,
                    ClassId = enrollment.ClassId,
                    ClassName = enrollment.Class?.Name,
                    EnrollmentDate = enrollment.EnrollmentDate,
                    CreatedDate = enrollment.CreatedDate,
                    UpdatedDate = enrollment.UpdatedDate
                }).ToList();

                return enrollmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching enrollments for class {classId}. {ex.Message}", ex);
                throw;
            }
        }
    }
}
