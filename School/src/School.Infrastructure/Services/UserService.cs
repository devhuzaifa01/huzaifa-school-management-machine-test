using Microsoft.Extensions.Logging;
using School.Application.Common;
using School.Application.Common.Errors;
using School.Application.Contracts.Persistence;
using School.Application.Contracts.Services;
using School.Application.Dtos;
using School.Application.Requests.User;
using School.Domain.Entities;
using School.Domain.Enums;

namespace School.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;

        public UserService(ILogger<UserService> logger, IUserRepository userRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<UserDto> CreateAsync(CreateUserRequest request)
        {
            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser is not null)
                {
                    throw new BusinessException("User with this email already exists");
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                User user = new()
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = hashedPassword,
                    Role = request.Role,
                    CreatedDate = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user);

                var createdUser = await _userRepository.GetByIdAsync(user.Id);

                UserDto userDto = new()
                {
                    Id = createdUser!.Id,
                    Name = createdUser.Name,
                    Email = createdUser.Email,
                    Role = createdUser.Role,
                    CreatedDate = createdUser.CreatedDate,
                    UpdatedDate = createdUser.UpdatedDate
                };

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while creating a new user. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user is null || user.IsDeleted == true)
                {
                    throw new NotFoundException("User not found");
                }

                UserDto userDto = new()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedDate = user.CreatedDate,
                    UpdatedDate = user.UpdatedDate
                };

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching user with id {id}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<UserDto> UpdateAsync(UpdateUserRequest request)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(request.Id);
                if (user is null || user.IsDeleted == true)
                {
                    throw new NotFoundException("User not found");
                }

                user.Name = request.Name;
                user.Role = request.Role;
                user.UpdatedDate = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                var updatedUser = await _userRepository.GetByIdAsync(user.Id);

                UserDto userDto = new()
                {
                    Id = updatedUser!.Id,
                    Name = updatedUser.Name,
                    Email = updatedUser.Email,
                    Role = updatedUser.Role,
                    CreatedDate = updatedUser.CreatedDate,
                    UpdatedDate = updatedUser.UpdatedDate
                };

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while updating user with id {request.Id}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user is null || user.IsDeleted == true)
                {
                    throw new NotFoundException("User not found");
                }

                user.IsDeleted = true;
                user.UpdatedDate = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while deleting user with id {id}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();

                List<UserDto> userDtos = users.Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedDate = user.CreatedDate,
                    UpdatedDate = user.UpdatedDate
                }).ToList();

                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching all users. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<UserDto>> GetByRoleAsync(string role)
        {
            try
            {
                var users = await _userRepository.GetByRoleAsync(role);

                List<UserDto> userDtos = users.Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedDate = user.CreatedDate,
                    UpdatedDate = user.UpdatedDate
                }).ToList();

                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching users by role {role}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<PagedResult<UserDto>> GetStudentsPagedAsync(PagingParameters parameters)
        {
            try
            {
                var (users, totalCount) = await _userRepository.GetStudentsPagedAsync(parameters.PageNumber, parameters.PageSize);

                var userDtos = users.Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedDate = user.CreatedDate,
                    UpdatedDate = user.UpdatedDate
                }).ToList();

                return new PagedResult<UserDto>
                {
                    Items = userDtos,
                    TotalCount = totalCount,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching students with pagination. {ex.Message}", ex);
                throw;
            }
        }
    }
}
