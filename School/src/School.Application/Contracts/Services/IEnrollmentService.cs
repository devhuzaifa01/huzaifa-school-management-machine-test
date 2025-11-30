using School.Application.Dtos;
using School.Application.Requests.Class;

namespace School.Application.Contracts.Services
{
    public interface IEnrollmentService
    {
        Task<StudentClassDto> EnrollStudentAsync(int classId, EnrollStudentRequest request, int teacherId);
        Task<List<StudentClassDto>> GetEnrollmentsByClassIdAsync(int classId, int teacherId);
        Task<List<StudentEnrolledClassDto>> GetEnrolledClassesByStudentIdAsync(int studentId);
    }
}
