using Microsoft.EntityFrameworkCore;
using School.Application.Contracts.Persistence;
using School.Domain.Entities;
using School.Infrastructure.Persistence;

namespace School.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SchoolDbContext _dbContext;

        public UserRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower()
                && (u.IsDeleted == null || u.IsDeleted == false));
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id
                && (u.IsDeleted == null || u.IsDeleted == false));
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbContext.Users.Where(u => u.IsDeleted == null || u.IsDeleted == false).ToListAsync();
        }

        public async Task<List<User>> GetByRoleAsync(string role)
        {
            return await _dbContext.Users.Where(u => u.Role.ToLower() == role.ToLower() && (u.IsDeleted == null || u.IsDeleted == false)).ToListAsync();
        }

        public async Task<User> AddAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
