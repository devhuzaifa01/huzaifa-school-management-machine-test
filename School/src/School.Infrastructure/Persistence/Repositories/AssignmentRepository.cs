using Microsoft.EntityFrameworkCore;
using School.Application.Contracts.Persistence;
using School.Domain.Entities;
using School.Infrastructure.Persistence;

namespace School.Infrastructure.Persistence.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly SchoolDbContext _dbContext;

        public AssignmentRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Assignment?> GetByIdAsync(int id)
        {
            return await _dbContext.Assignments
                .Include(a => a.Class)
                .Include(a => a.CreatedByTeacher)
                .FirstOrDefaultAsync(a => a.Id == id && (a.IsDeleted == null || a.IsDeleted == false));
        }

        public async Task<Assignment?> GetByIdWithClassAsync(int id)
        {
            return await _dbContext.Assignments
                .Include(a => a.Class)
                .Include(a => a.CreatedByTeacher)
                .FirstOrDefaultAsync(a => a.Id == id && (a.IsDeleted == null || a.IsDeleted == false));
        }

        public async Task<List<Assignment>> GetByClassIdAsync(int classId)
        {
            return await _dbContext.Assignments
                .Include(a => a.Class)
                .Include(a => a.CreatedByTeacher)
                .Where(a => a.ClassId == classId && (a.IsDeleted == null || a.IsDeleted == false))
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Assignment>> GetByClassIdAndTeacherIdAsync(int classId, int teacherId)
        {
            return await _dbContext.Assignments
                .Include(a => a.Class)
                .Include(a => a.CreatedByTeacher)
                .Where(a => a.ClassId == classId && a.CreatedByTeacherId == teacherId && (a.IsDeleted == null || a.IsDeleted == false))
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task<Assignment> AddAsync(Assignment assignment)
        {
            _dbContext.Assignments.Add(assignment);
            await _dbContext.SaveChangesAsync();
            return assignment;
        }

        public async Task<(List<Assignment> Items, int TotalCount)> GetByTeacherIdPagedAsync(int teacherId, int pageNumber, int pageSize)
        {
            var query = _dbContext.Assignments
                .Include(a => a.Class)
                .Include(a => a.CreatedByTeacher)
                .Where(a => a.CreatedByTeacherId == teacherId && (a.IsDeleted == null || a.IsDeleted == false));

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
