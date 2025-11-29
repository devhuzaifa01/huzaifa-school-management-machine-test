using School.Application.Dtos;
using School.Application.Requests.Teacher;

namespace School.Application.Contracts.Services
{
    public interface IAssignmentService
    {
        Task<AssignmentDto> CreateAsync(CreateAssignmentRequest request, int teacherId);
        Task<List<AssignmentDto>> GetByClassIdAsync(int classId, int teacherId);
    }
}
