using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using School.Application.Common;
using School.Application.Contracts.Persistence;
using School.Application.Contracts.Services;
using School.Application.Dtos;
using School.Application.Requests.Course;
using School.Domain.Entities;

namespace School.Infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly ILogger<CourseService> _logger;
        private readonly ICourseRepository _courseRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMemoryCache _cache;
        private readonly CacheSettings _cacheSettings;

        public CourseService(ILogger<CourseService> logger,
            ICourseRepository courseRepository,
            IDepartmentRepository departmentRepository,
            IMemoryCache cache,
            IOptions<CacheSettings> cacheSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _cacheSettings = cacheSettings?.Value ?? throw new ArgumentNullException(nameof(cacheSettings));
        }

        public async Task<CourseDto> CreateAsync(CreateCourseRequest request)
        {
            try
            {
                var existingCourse = await _courseRepository.GetByCodeAndDepartmentIdAsync(request.Code, request.DepartmentId);
                if (existingCourse is not null)
                {
                    throw new InvalidOperationException("Course code must be unique per department");
                }

                var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);
                if (department is null)
                {
                    throw new InvalidOperationException("Department not found");
                }

                Course course = new()
                {
                    Name = request.Name,
                    Code = request.Code,
                    Description = request.Description,
                    DepartmentId = request.DepartmentId,
                    Credits = request.Credits,
                    CreatedDate = DateTime.UtcNow
                };

                await _courseRepository.AddAsync(course);

                var createdCourse = await _courseRepository.GetByIdAsync(course.Id);

                CourseDto courseDto = new()
                {
                    Id = createdCourse!.Id,
                    Name = createdCourse.Name,
                    Code = createdCourse.Code,
                    Description = createdCourse.Description,
                    DepartmentId = createdCourse.DepartmentId,
                    DepartmentName = createdCourse.Department?.Name,
                    Credits = createdCourse.Credits,
                    CreatedDate = createdCourse.CreatedDate,
                    UpdatedDate = createdCourse.UpdatedDate
                };

                _cache.Remove(CacheKeys.CourseList);

                return courseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while creating a new course. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<CourseDto> GetByIdAsync(int id)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(id);
                if (course is null || course.IsDeleted == true)
                {
                    throw new InvalidOperationException("Course not found");
                }

                CourseDto courseDto = new()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Code = course.Code,
                    Description = course.Description,
                    DepartmentId = course.DepartmentId,
                    DepartmentName = course.Department?.Name,
                    Credits = course.Credits,
                    CreatedDate = course.CreatedDate,
                    UpdatedDate = course.UpdatedDate
                };

                return courseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching course with id {id}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<CourseDto>> GetAllAsync()
        {
            try
            {
                if (_cache.TryGetValue(CacheKeys.CourseList, out List<CourseDto>? cachedCourses) && cachedCourses != null)
                {
                    return cachedCourses;
                }

                var courses = await _courseRepository.GetAllAsync();

                List<CourseDto> courseDtos = courses.Select(course => new CourseDto
                {
                    Id = course.Id,
                    Name = course.Name,
                    Code = course.Code,
                    Description = course.Description,
                    DepartmentId = course.DepartmentId,
                    DepartmentName = course.Department?.Name,
                    Credits = course.Credits,
                    CreatedDate = course.CreatedDate,
                    UpdatedDate = course.UpdatedDate
                }).ToList();

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheSettings.CourseExpirySeconds)
                };

                _cache.Set(CacheKeys.CourseList, courseDtos, cacheOptions);

                return courseDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching all courses. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<CourseDto> UpdateAsync(UpdateCourseRequest request)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(request.Id);
                if (course is null || course.IsDeleted == true)
                {
                    throw new InvalidOperationException("Course not found");
                }

                var existingCourse = await _courseRepository.GetByCodeAndDepartmentIdAsync(request.Code, request.DepartmentId);
                if (existingCourse is not null && existingCourse.Id != request.Id)
                {
                    throw new InvalidOperationException("Course code must be unique per department");
                }

                var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);
                if (department is null)
                {
                    throw new InvalidOperationException("Department not found");
                }

                course.Name = request.Name;
                course.Code = request.Code;
                course.Description = request.Description;
                course.DepartmentId = request.DepartmentId;
                course.Credits = request.Credits;
                course.UpdatedDate = DateTime.UtcNow;

                await _courseRepository.UpdateAsync(course);

                var updatedCourse = await _courseRepository.GetByIdAsync(course.Id);

                CourseDto courseDto = new()
                {
                    Id = updatedCourse!.Id,
                    Name = updatedCourse.Name,
                    Code = updatedCourse.Code,
                    Description = updatedCourse.Description,
                    DepartmentId = updatedCourse.DepartmentId,
                    DepartmentName = updatedCourse.Department?.Name,
                    Credits = updatedCourse.Credits,
                    CreatedDate = updatedCourse.CreatedDate,
                    UpdatedDate = updatedCourse.UpdatedDate
                };

                _cache.Remove(CacheKeys.CourseList);

                return courseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while updating course with id {request.Id}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(id);
                if (course is null || course.IsDeleted == true)
                {
                    throw new InvalidOperationException("Course not found");
                }

                course.IsDeleted = true;
                course.IsActive = false;
                course.UpdatedDate = DateTime.UtcNow;
                await _courseRepository.UpdateAsync(course);

                _cache.Remove(CacheKeys.CourseList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while deleting course with id {id}. {ex.Message}", ex);
                throw;
            }
        }
    }
}
