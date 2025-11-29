using School.Domain.Entities;

namespace School.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<List<User>> GetAllAsync();
        Task<List<User>> GetByRoleAsync(string role);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
    }
}
