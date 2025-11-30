using School.Domain.Entities;

namespace School.Application.Contracts.Persistence
{
    public interface IAssignmentRepository
    {
        Task<Assignment?> GetByIdAsync(int id);
        Task<Assignment?> GetByIdWithClassAsync(int id);
        Task<List<Assignment>> GetByClassIdAsync(int classId);
        Task<List<Assignment>> GetByClassIdAndTeacherIdAsync(int classId, int teacherId);
        Task<Assignment> AddAsync(Assignment assignment);
    }
}
