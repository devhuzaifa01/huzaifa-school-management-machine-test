using Microsoft.Extensions.Logging;
using School.Application.Contracts.Persistence;
using School.Application.Contracts.Services;
using School.Application.Dtos;
using School.Application.Requests.Teacher;
using School.Domain.Entities;
using School.Domain.Enums;

namespace School.Infrastructure.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ILogger<AttendanceService> _logger;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IClassRepository _classRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public AttendanceService(ILogger<AttendanceService> logger,
            IAttendanceRepository attendanceRepository,
            IClassRepository classRepository,
            IUserRepository userRepository,
            IEnrollmentRepository enrollmentRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _attendanceRepository = attendanceRepository ?? throw new ArgumentNullException(nameof(attendanceRepository));
            _classRepository = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
        }

        public async Task<AttendanceDto> MarkAttendanceAsync(MarkAttendanceRequest request, int teacherId)
        {
            try
            {
                var classEntity = await _classRepository.GetByIdAndTeacherIdAsync(request.ClassId, teacherId);
                if (classEntity is null)
                {
                    throw new InvalidOperationException("Class not found or you do not have permission to mark attendance for this class");
                }

                var student = await _userRepository.GetByIdAsync(request.StudentId);
                if (student is null)
                {
                    throw new InvalidOperationException("Student not found");
                }

                if (student.Role != UserRole.Student.ToString())
                {
                    throw new InvalidOperationException("User is not a student");
                }

                var isEnrolled = await _enrollmentRepository.IsStudentEnrolledAsync(request.StudentId, request.ClassId);
                if (!isEnrolled)
                {
                    throw new InvalidOperationException("Student is not enrolled in this class");
                }

                var existingAttendance = await _attendanceRepository.GetByClassIdStudentIdAndDateAsync(
                    request.ClassId, request.StudentId, request.Date);
                
                if (existingAttendance is not null)
                {
                    throw new InvalidOperationException("Attendance for this student on this date already exists");
                }

                Attendance attendance = new()
                {
                    ClassId = request.ClassId,
                    StudentId = request.StudentId,
                    Date = request.Date.Date,
                    Status = request.Status,
                    MarkedByTeacherId = teacherId,
                    CreatedDate = DateTime.UtcNow
                };

                await _attendanceRepository.AddAsync(attendance);

                var markedAttendance = await _attendanceRepository.GetByIdAsync(attendance.Id);

                AttendanceDto attendanceDto = new()
                {
                    Id = markedAttendance!.Id,
                    ClassId = markedAttendance.ClassId,
                    ClassName = markedAttendance.Class?.Name,
                    StudentId = markedAttendance.StudentId,
                    StudentName = markedAttendance.Student?.Name,
                    StudentEmail = markedAttendance.Student?.Email,
                    Date = markedAttendance.Date,
                    Status = markedAttendance.Status,
                    MarkedByTeacherId = markedAttendance.MarkedByTeacherId,
                    MarkedByTeacherName = markedAttendance.MarkedByTeacher?.Name,
                    CreatedDate = markedAttendance.CreatedDate
                };

                return attendanceDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while marking attendance. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<AttendanceDto>> GetAttendanceHistoryAsync(int classId, int teacherId)
        {
            try
            {
                var classEntity = await _classRepository.GetByIdAndTeacherIdAsync(classId, teacherId);
                if (classEntity is null)
                {
                    throw new InvalidOperationException("Class not found or you do not have permission to view attendance for this class");
                }

                var attendances = await _attendanceRepository.GetByClassIdAsync(classId);

                List<AttendanceDto> attendanceDtos = attendances.Select(attendance => new AttendanceDto
                {
                    Id = attendance.Id,
                    ClassId = attendance.ClassId,
                    ClassName = attendance.Class?.Name,
                    StudentId = attendance.StudentId,
                    StudentName = attendance.Student?.Name,
                    StudentEmail = attendance.Student?.Email,
                    Date = attendance.Date,
                    Status = attendance.Status,
                    MarkedByTeacherId = attendance.MarkedByTeacherId,
                    MarkedByTeacherName = attendance.MarkedByTeacher?.Name,
                    CreatedDate = attendance.CreatedDate
                }).ToList();

                return attendanceDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching attendance history for class {classId}. {ex.Message}", ex);
                throw;
            }
        }
    }
}
