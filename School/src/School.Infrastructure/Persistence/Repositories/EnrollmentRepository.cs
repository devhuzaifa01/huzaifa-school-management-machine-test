using Microsoft.EntityFrameworkCore;
using School.Application.Contracts.Persistence;
using School.Domain.Entities;
using School.Infrastructure.Persistence;

namespace School.Infrastructure.Persistence.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly SchoolDbContext _dbContext;

        public EnrollmentRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<StudentClass?> GetByStudentIdAndClassIdAsync(int studentId, int classId)
        {
            return await _dbContext.StudentClasses.FirstOrDefaultAsync(sc => sc.StudentId == studentId 
                    && sc.ClassId == classId 
                    && (sc.IsDeleted == null || sc.IsDeleted == false));
        }

        public async Task<bool> IsStudentEnrolledAsync(int studentId, int classId)
        {
            return await _dbContext.StudentClasses
                .AnyAsync(sc => sc.StudentId == studentId 
                    && sc.ClassId == classId 
                    && (sc.IsDeleted == null || sc.IsDeleted == false));
        }

        public async Task<StudentClass?> GetByIdAsync(int id)
        {
            return await _dbContext.StudentClasses
                .Include(sc => sc.Student)
                .Include(sc => sc.Class)
                .FirstOrDefaultAsync(sc => sc.Id == id && (sc.IsDeleted == null || sc.IsDeleted == false));
        }

        public async Task<List<StudentClass>> GetByClassIdAsync(int classId)
        {
            return await _dbContext.StudentClasses
                .Include(sc => sc.Student)
                .Include(sc => sc.Class)
                .Where(sc => sc.ClassId == classId && (sc.IsDeleted == null || sc.IsDeleted == false))
                .OrderBy(sc => sc.Student!.Name)
                .ToListAsync();
        }

        public async Task<StudentClass> AddAsync(StudentClass studentClass)
        {
            _dbContext.StudentClasses.Add(studentClass);
            await _dbContext.SaveChangesAsync();
            return studentClass;
        }
    }
}
