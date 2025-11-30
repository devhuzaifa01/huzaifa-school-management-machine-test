using School.Application.Dtos;
using School.Application.Requests.Course;

namespace School.Application.Contracts.Services
{
    public interface ICourseService
    {
        Task<CourseDto> CreateAsync(CreateCourseRequest request);
        Task<CourseDto> GetByIdAsync(int id);
        Task<List<CourseDto>> GetAllAsync();
        Task<CourseDto> UpdateAsync(UpdateCourseRequest request);
        Task DeleteAsync(int id);
    }
}
