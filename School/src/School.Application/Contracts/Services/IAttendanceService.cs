using School.Application.Dtos;
using School.Application.Requests.Teacher;

namespace School.Application.Contracts.Services
{
    public interface IAttendanceService
    {
        Task<AttendanceDto> MarkAttendanceAsync(MarkAttendanceRequest request, int teacherId);
        Task<List<AttendanceDto>> GetAttendanceHistoryAsync(int classId, int teacherId);
    }
}
