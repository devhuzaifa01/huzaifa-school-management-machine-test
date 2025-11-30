using Microsoft.AspNetCore.Http;
using School.Application.Dtos;
using School.Application.Requests.Teacher;

namespace School.Application.Contracts.Services
{
    public interface IAssignmentService
    {
        Task<AssignmentDto> CreateAsync(CreateAssignmentRequest request, int teacherId);
        Task<List<AssignmentDto>> GetByClassIdAsync(int classId, int teacherId);
        Task<StudentAssignmentDto> GetByIdForStudentAsync(int id, int studentId);
        Task<SubmissionDto> SubmitAssignmentAsync(int assignmentId, IFormFile file, int studentId, string webRootPath);
        Task<SubmissionDto> GradeSubmissionAsync(int submissionId, GradeSubmissionRequest request, int teacherId);
        Task<SubmissionDto> GetSubmissionByIdForStudentAsync(int submissionId, int studentId);
        Task<List<SubmissionDto>> GetSubmissionsByStudentIdAsync(int studentId);
    }
}
