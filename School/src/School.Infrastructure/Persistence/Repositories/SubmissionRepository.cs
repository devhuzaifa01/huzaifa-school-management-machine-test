using Microsoft.EntityFrameworkCore;
using School.Application.Contracts.Persistence;
using School.Domain.Entities;
using School.Infrastructure.Persistence;

namespace School.Infrastructure.Persistence.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly SchoolDbContext _dbContext;

        public SubmissionRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Submission?> GetByAssignmentIdAndStudentIdAsync(int assignmentId, int studentId)
        {
            return await _dbContext.Submissions
                .Include(s => s.Assignment)
                .Include(s => s.Student)
                .Include(s => s.GradedByTeacher)
                .FirstOrDefaultAsync(s => s.AssignmentId == assignmentId 
                    && s.StudentId == studentId 
                    && (s.IsDeleted == null || s.IsDeleted == false));
        }

        public async Task<Submission?> GetByIdAsync(int id)
        {
            return await _dbContext.Submissions
                .Include(s => s.Assignment)
                .Include(s => s.Student)
                .Include(s => s.GradedByTeacher)
                .FirstOrDefaultAsync(s => s.Id == id && (s.IsDeleted == null || s.IsDeleted == false));
        }

        public async Task<Submission?> GetByIdWithAssignmentAndClassAsync(int id)
        {
            return await _dbContext.Submissions
                .Include(s => s.Assignment)
                    .ThenInclude(a => a!.Class)
                .Include(s => s.Student)
                .Include(s => s.GradedByTeacher)
                .FirstOrDefaultAsync(s => s.Id == id && (s.IsDeleted == null || s.IsDeleted == false));
        }

        public async Task<List<Submission>> GetByStudentIdAsync(int studentId)
        {
            return await _dbContext.Submissions
                .Include(s => s.Assignment)
                    .ThenInclude(a => a!.Class)
                .Include(s => s.Student)
                .Include(s => s.GradedByTeacher)
                .Where(s => s.StudentId == studentId 
                    && (s.IsDeleted == null || s.IsDeleted == false)
                    && (s.Assignment == null || (s.Assignment.IsDeleted == null || s.Assignment.IsDeleted == false)))
                .OrderByDescending(s => s.SubmittedDate)
                .ToListAsync();
        }

        public async Task<Submission> AddAsync(Submission submission)
        {
            _dbContext.Submissions.Add(submission);
            await _dbContext.SaveChangesAsync();
            return submission;
        }

        public async Task<Submission> UpdateAsync(Submission submission)
        {
            _dbContext.Submissions.Update(submission);
            await _dbContext.SaveChangesAsync();
            return submission;
        }
    }
}
