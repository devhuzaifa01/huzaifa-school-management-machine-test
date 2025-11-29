using School.Domain.Entities;

namespace School.Application.Contracts.Persistence
{
    public interface IEnrollmentRepository
    {
        Task<StudentClass?> GetByStudentIdAndClassIdAsync(int studentId, int classId);
        Task<bool> IsStudentEnrolledAsync(int studentId, int classId);
        Task<StudentClass?> GetByIdAsync(int id);
        Task<List<StudentClass>> GetByClassIdAsync(int classId);
        Task<StudentClass> AddAsync(StudentClass studentClass);
    }
}
