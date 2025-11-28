using School.Domain.Entities;

namespace School.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
    }
}
