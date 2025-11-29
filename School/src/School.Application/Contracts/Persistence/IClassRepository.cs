using School.Domain.Entities;

namespace School.Application.Contracts.Persistence
{
    public interface IClassRepository
    {
        Task<Class?> GetByIdAsync(int id);
        Task<Class?> GetByIdAndTeacherIdAsync(int id, int teacherId);
        Task<List<Class>> GetAllAsync();
        Task<List<Class>> GetAllByTeacherIdAsync(int teacherId);
        Task<Class> AddAsync(Class classEntity);
        Task<Class> UpdateAsync(Class classEntity);
    }
}
