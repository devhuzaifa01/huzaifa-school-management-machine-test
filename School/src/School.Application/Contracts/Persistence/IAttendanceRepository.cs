using School.Domain.Entities;

namespace School.Application.Contracts.Persistence
{
    public interface IAttendanceRepository
    {
        Task<Attendance?> GetByClassIdStudentIdAndDateAsync(int classId, int studentId, DateTime date);
        Task<Attendance?> GetByIdAsync(int id);
        Task<List<Attendance>> GetByClassIdAsync(int classId);
        Task<Attendance> AddAsync(Attendance attendance);
    }
}
