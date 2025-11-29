using Microsoft.Extensions.Logging;
using School.Application.Contracts.Persistence;
using School.Application.Contracts.Services;
using School.Application.Dtos;
using School.Application.Requests.Teacher;
using School.Domain.Entities;
using School.Domain.Enums;

namespace School.Infrastructure.Services
{
    public class ClassService : IClassService
    {
        private readonly ILogger<ClassService> _logger;
        private readonly IClassRepository _classRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;

        public ClassService(ILogger<ClassService> logger,
            IClassRepository classRepository,
            ICourseRepository courseRepository,
            IUserRepository userRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _classRepository = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<ClassDto> CreateAsync(CreateClassRequest request, int teacherId)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(request.CourseId);
                if (course is null)
                {
                    throw new InvalidOperationException("Course not found");
                }

                var teacher = await _userRepository.GetByIdAsync(teacherId);
                if (teacher is null)
                {
                    throw new InvalidOperationException("Teacher not found");
                }

                if (teacher.Role != UserRole.Teacher.ToString())
                {
                    throw new InvalidOperationException("Only teachers can be assigned to a class");
                }

                if (request.StartDate >= request.EndDate)
                {
                    throw new InvalidOperationException("StartDate must be before EndDate");
                }

                Class classEntity = new()
                {
                    Name = request.Name,
                    CourseId = request.CourseId,
                    TeacherId = teacherId,
                    Semester = request.Semester,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    IsActive = request.IsActive,
                    CreatedDate = DateTime.UtcNow
                };

                await _classRepository.AddAsync(classEntity);

                var createdClass = await _classRepository.GetByIdAsync(classEntity.Id);

                ClassDto classDto = new()
                {
                    Id = createdClass!.Id,
                    Name = createdClass.Name,
                    CourseId = createdClass.CourseId,
                    CourseName = createdClass.Course?.Name,
                    CourseCode = createdClass.Course?.Code,
                    TeacherId = createdClass.TeacherId,
                    TeacherName = createdClass.Teacher?.Name,
                    TeacherEmail = createdClass.Teacher?.Email,
                    Semester = createdClass.Semester,
                    StartDate = createdClass.StartDate,
                    EndDate = createdClass.EndDate,
                    IsActive = createdClass.IsActive,
                    CreatedDate = createdClass.CreatedDate,
                    UpdatedDate = createdClass.UpdatedDate
                };

                return classDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while creating a new class. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<ClassDto> GetByIdAsync(int id, int teacherId)
        {
            try
            {
                var classEntity = await _classRepository.GetByIdAndTeacherIdAsync(id, teacherId);
                if (classEntity is null)
                {
                    throw new InvalidOperationException("Class not found");
                }

                ClassDto classDto = new()
                {
                    Id = classEntity.Id,
                    Name = classEntity.Name,
                    CourseId = classEntity.CourseId,
                    CourseName = classEntity.Course?.Name,
                    CourseCode = classEntity.Course?.Code,
                    TeacherId = classEntity.TeacherId,
                    TeacherName = classEntity.Teacher?.Name,
                    TeacherEmail = classEntity.Teacher?.Email,
                    Semester = classEntity.Semester,
                    StartDate = classEntity.StartDate,
                    EndDate = classEntity.EndDate,
                    IsActive = classEntity.IsActive,
                    CreatedDate = classEntity.CreatedDate,
                    UpdatedDate = classEntity.UpdatedDate
                };

                return classDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching class with id {id}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<ClassDto>> GetAllAsync(int teacherId)
        {
            try
            {
                var classes = await _classRepository.GetAllByTeacherIdAsync(teacherId);

                List<ClassDto> classDtos = classes.Select(classEntity => new ClassDto
                {
                    Id = classEntity.Id,
                    Name = classEntity.Name,
                    CourseId = classEntity.CourseId,
                    CourseName = classEntity.Course?.Name,
                    CourseCode = classEntity.Course?.Code,
                    TeacherId = classEntity.TeacherId,
                    TeacherName = classEntity.Teacher?.Name,
                    TeacherEmail = classEntity.Teacher?.Email,
                    Semester = classEntity.Semester,
                    StartDate = classEntity.StartDate,
                    EndDate = classEntity.EndDate,
                    IsActive = classEntity.IsActive,
                    CreatedDate = classEntity.CreatedDate,
                    UpdatedDate = classEntity.UpdatedDate
                }).ToList();

                return classDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching all classes. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<ClassDto> UpdateAsync(UpdateClassRequest request, int teacherId)
        {
            try
            {
                var classEntity = await _classRepository.GetByIdAsync(request.Id);
                if (classEntity is null || classEntity.IsDeleted == true)
                {
                    throw new InvalidOperationException("Class not found");
                }

                if (classEntity.TeacherId != teacherId)
                {
                    throw new UnauthorizedAccessException("You can only update your own classes");
                }

                var course = await _courseRepository.GetByIdAsync(request.CourseId);
                if (course is null)
                {
                    throw new InvalidOperationException("Course not found");
                }

                var teacher = await _userRepository.GetByIdAsync(teacherId);
                if (teacher is null)
                {
                    throw new InvalidOperationException("Teacher not found");
                }

                if (teacher.Role != UserRole.Teacher.ToString())
                {
                    throw new InvalidOperationException("Only teachers can update classes");
                }

                if (request.StartDate >= request.EndDate)
                {
                    throw new InvalidOperationException("StartDate must be before EndDate");
                }

                classEntity.Name = request.Name;
                classEntity.CourseId = request.CourseId;
                classEntity.TeacherId = teacherId;
                classEntity.Semester = request.Semester;
                classEntity.StartDate = request.StartDate;
                classEntity.EndDate = request.EndDate;
                classEntity.IsActive = request.IsActive;
                classEntity.UpdatedDate = DateTime.UtcNow;

                await _classRepository.UpdateAsync(classEntity);

                var updatedClass = await _classRepository.GetByIdAsync(classEntity.Id);

                ClassDto classDto = new()
                {
                    Id = updatedClass!.Id,
                    Name = updatedClass.Name,
                    CourseId = updatedClass.CourseId,
                    CourseName = updatedClass.Course?.Name,
                    CourseCode = updatedClass.Course?.Code,
                    TeacherId = updatedClass.TeacherId,
                    TeacherName = updatedClass.Teacher?.Name,
                    TeacherEmail = updatedClass.Teacher?.Email,
                    Semester = updatedClass.Semester,
                    StartDate = updatedClass.StartDate,
                    EndDate = updatedClass.EndDate,
                    IsActive = updatedClass.IsActive,
                    CreatedDate = updatedClass.CreatedDate,
                    UpdatedDate = updatedClass.UpdatedDate
                };

                return classDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while updating class with id {request.Id}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task DeactivateAsync(int id)
        {
            try
            {
                var classEntity = await _classRepository.GetByIdAsync(id);
                if (classEntity is null || classEntity.IsDeleted == true)
                {
                    throw new InvalidOperationException("Class not found");
                }

                classEntity.IsActive = false;
                classEntity.UpdatedDate = DateTime.UtcNow;

                await _classRepository.UpdateAsync(classEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while deactivating class with id {id}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task ActivateAsync(int id)
        {
            try
            {
                var classEntity = await _classRepository.GetByIdAsync(id);
                if (classEntity is null || classEntity.IsDeleted == true)
                {
                    throw new InvalidOperationException("Class not found");
                }

                classEntity.IsActive = true;
                classEntity.UpdatedDate = DateTime.UtcNow;

                await _classRepository.UpdateAsync(classEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while activating class with id {id}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<ClassDto> GetByIdForAdminAsync(int id)
        {
            try
            {
                var classEntity = await _classRepository.GetByIdAsync(id);
                if (classEntity is null || classEntity.IsDeleted == true)
                {
                    throw new InvalidOperationException("Class not found");
                }

                ClassDto classDto = new()
                {
                    Id = classEntity.Id,
                    Name = classEntity.Name,
                    CourseId = classEntity.CourseId,
                    CourseName = classEntity.Course?.Name,
                    CourseCode = classEntity.Course?.Code,
                    TeacherId = classEntity.TeacherId,
                    TeacherName = classEntity.Teacher?.Name,
                    TeacherEmail = classEntity.Teacher?.Email,
                    Semester = classEntity.Semester,
                    StartDate = classEntity.StartDate,
                    EndDate = classEntity.EndDate,
                    IsActive = classEntity.IsActive,
                    CreatedDate = classEntity.CreatedDate,
                    UpdatedDate = classEntity.UpdatedDate
                };

                return classDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching class with id {id} for admin. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<ClassDto>> GetAllForAdminAsync()
        {
            try
            {
                var classes = await _classRepository.GetAllAsync();

                List<ClassDto> classDtos = classes.Select(classEntity => new ClassDto
                {
                    Id = classEntity.Id,
                    Name = classEntity.Name,
                    CourseId = classEntity.CourseId,
                    CourseName = classEntity.Course?.Name,
                    CourseCode = classEntity.Course?.Code,
                    TeacherId = classEntity.TeacherId,
                    TeacherName = classEntity.Teacher?.Name,
                    TeacherEmail = classEntity.Teacher?.Email,
                    Semester = classEntity.Semester,
                    StartDate = classEntity.StartDate,
                    EndDate = classEntity.EndDate,
                    IsActive = classEntity.IsActive,
                    CreatedDate = classEntity.CreatedDate,
                    UpdatedDate = classEntity.UpdatedDate
                }).ToList();

                return classDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching all classes for admin. {ex.Message}", ex);
                throw;
            }
        }
    }
}
