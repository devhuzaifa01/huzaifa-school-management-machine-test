using School.Domain.Entities;

namespace School.Application.Contracts.Persistence
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetByNameAsync(string name);
        Task<Department?> GetByIdAsync(int id);
        Task<Department> AddAsync(Department department);
        Task<Department> UpdateAsync(Department department);
    }
}
