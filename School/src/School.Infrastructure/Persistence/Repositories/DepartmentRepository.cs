using Microsoft.EntityFrameworkCore;
using School.Application.Contracts.Persistence;
using School.Domain.Entities;
using School.Infrastructure.Persistence;

namespace School.Infrastructure.Persistence.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly SchoolDbContext _dbContext;

        public DepartmentRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Department?> GetByNameAsync(string name)
        {
            return await _dbContext.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Name.ToLower() == name.ToLower() && (d.IsDeleted == null || d.IsDeleted == false));
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _dbContext.Departments
                .Include(d => d.HeadOfDepartment)
                .FirstOrDefaultAsync(d => d.Id == id && (d.IsDeleted == null || d.IsDeleted == false));
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _dbContext.Departments
                .Include(d => d.HeadOfDepartment)
                .Where(d => d.IsDeleted == null || d.IsDeleted == false)
                .ToListAsync();
        }
         
        public async Task<Department> AddAsync(Department department)
        {
            _dbContext.Departments.Add(department);
            await _dbContext.SaveChangesAsync();
            return department;
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            _dbContext.Departments.Update(department);
            await _dbContext.SaveChangesAsync();
            return department;
        }
    }
}
