using School.Application.Dtos;
using School.Application.Requests.Attendance;

namespace School.Application.Contracts.Services
{
    public interface IAttendanceService
    {
        Task<AttendanceDto> MarkAttendanceAsync(MarkAttendanceRequest request, int teacherId);
        Task<List<AttendanceDto>> GetAttendanceHistoryAsync(int classId, int teacherId);
        Task<List<AttendanceDto>> GetAttendanceByStudentIdAsync(int studentId);
        Task<List<AttendanceDto>> GetAttendanceByStudentIdAndClassIdAsync(int studentId, int classId);
    }
}
