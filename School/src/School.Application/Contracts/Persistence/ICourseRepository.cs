using School.Domain.Entities;

namespace School.Application.Contracts.Persistence
{
    public interface ICourseRepository
    {
        Task<Course?> GetByCodeAndDepartmentIdAsync(string code, int departmentId);
        Task<Course?> GetByIdAsync(int id);
        Task<List<Course>> GetAllAsync();
        Task<Course> AddAsync(Course course);
        Task<Course> UpdateAsync(Course course);
    }
}
