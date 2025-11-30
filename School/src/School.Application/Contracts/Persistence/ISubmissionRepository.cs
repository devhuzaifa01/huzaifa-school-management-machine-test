using School.Domain.Entities;

namespace School.Application.Contracts.Persistence
{
    public interface ISubmissionRepository
    {
        Task<Submission?> GetByAssignmentIdAndStudentIdAsync(int assignmentId, int studentId);
        Task<Submission?> GetByIdAsync(int id);
        Task<Submission> AddAsync(Submission submission);
    }
}
