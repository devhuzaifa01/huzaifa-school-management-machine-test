using School.Application.Dtos;
using School.Application.Requests.User;

namespace School.Application.Contracts.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateAsync(CreateUserRequest request);
        Task<UserDto> GetByIdAsync(int id);
        Task<List<UserDto>> GetAllAsync();
        Task<List<UserDto>> GetByRoleAsync(string role);
        Task<UserDto> UpdateAsync(UpdateUserRequest request);
        Task DeleteAsync(int id);
    }
}
