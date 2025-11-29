using Microsoft.EntityFrameworkCore;
using School.Application.Contracts.Persistence;
using School.Domain.Entities;
using School.Infrastructure.Persistence;

namespace School.Infrastructure.Persistence.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SchoolDbContext _dbContext;

        public CourseRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Course?> GetByCodeAndDepartmentIdAsync(string code, int departmentId)
        {
            return await _dbContext.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Code.ToLower() == code.ToLower() 
                    && c.DepartmentId == departmentId 
                    && (c.IsDeleted == null || c.IsDeleted == false));
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _dbContext.Courses
                .Include(c => c.Department)
                .FirstOrDefaultAsync(c => c.Id == id && (c.IsDeleted == null || c.IsDeleted == false));
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _dbContext.Courses
                .Include(c => c.Department)
                .Where(c => c.IsDeleted == null || c.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Course> AddAsync(Course course)
        {
            _dbContext.Courses.Add(course);
            await _dbContext.SaveChangesAsync();
            return course;
        }

        public async Task<Course> UpdateAsync(Course course)
        {
            _dbContext.Courses.Update(course);
            await _dbContext.SaveChangesAsync();
            return course;
        }
    }
}
