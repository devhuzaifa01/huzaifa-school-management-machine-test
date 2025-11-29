using Microsoft.EntityFrameworkCore;
using School.Application.Contracts.Persistence;
using School.Domain.Entities;
using School.Infrastructure.Persistence;

namespace School.Infrastructure.Persistence.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly SchoolDbContext _dbContext;

        public ClassRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Class?> GetByIdAsync(int id)
        {
            return await _dbContext.Classes
                .Include(c => c.Course)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id && (c.IsDeleted == null || c.IsDeleted == false));
        }

        public async Task<Class?> GetByIdAndTeacherIdAsync(int id, int teacherId)
        {
            return await _dbContext.Classes
                .Include(c => c.Course)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id && c.TeacherId == teacherId && (c.IsDeleted == null || c.IsDeleted == false));
        }

        public async Task<List<Class>> GetAllAsync()
        {
            return await _dbContext.Classes
                .Include(c => c.Course)
                .Include(c => c.Teacher)
                .Where(c => c.IsDeleted == null || c.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<List<Class>> GetAllByTeacherIdAsync(int teacherId)
        {
            return await _dbContext.Classes
                .Include(c => c.Course)
                .Include(c => c.Teacher)
                .Where(c => c.TeacherId == teacherId && (c.IsDeleted == null || c.IsDeleted == false))
                .ToListAsync();
        }

        public async Task<Class> AddAsync(Class classEntity)
        {
            _dbContext.Classes.Add(classEntity);
            await _dbContext.SaveChangesAsync();
            return classEntity;
        }

        public async Task<Class> UpdateAsync(Class classEntity)
        {
            _dbContext.Classes.Update(classEntity);
            await _dbContext.SaveChangesAsync();
            return classEntity;
        }
    }
}
