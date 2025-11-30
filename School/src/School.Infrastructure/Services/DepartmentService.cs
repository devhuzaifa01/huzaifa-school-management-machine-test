using Microsoft.Extensions.Logging;
using School.Application.Contracts.Persistence;
using School.Application.Contracts.Services;
using School.Application.Dtos;
using School.Application.Requests.Department;
using School.Domain.Entities;
using School.Domain.Enums;

namespace School.Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ILogger<DepartmentService> _logger;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;

        public DepartmentService(ILogger<DepartmentService> logger,
            IDepartmentRepository departmentRepository,
            IUserRepository userRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentRequest request)
        {
            try
            {
                var existingDepartment = await _departmentRepository.GetByNameAsync(request.Name);
                if (existingDepartment is not null)
                {
                    throw new InvalidOperationException("Department name must be unique");
                }

                if (request.HeadOfDepartmentId.HasValue)
                {
                    var teacher = await _userRepository.GetByIdAsync(request.HeadOfDepartmentId.Value);
                    if (teacher is null)
                    {
                        throw new InvalidOperationException("Head of Department not found");
                    }

                    if (teacher.Role != UserRole.Teacher.ToString())
                    {
                        throw new InvalidOperationException("Only teachers can be assigned as Head of Department");
                    }
                }

                Department department = new()
                {
                    Name = request.Name,
                    Description = request.Description,
                    HeadOfDepartmentId = request.HeadOfDepartmentId,
                    CreatedDate = DateTime.UtcNow
                };

                await _departmentRepository.AddAsync(department);

                var createdDepartment = await _departmentRepository.GetByIdAsync(department.Id);

                DepartmentDto departmentDto = new()
                {
                    Id = createdDepartment!.Id,
                    Name = createdDepartment.Name,
                    Description = createdDepartment.Description,
                    HeadOfDepartmentId = createdDepartment.HeadOfDepartmentId,
                    HeadOfDepartmentName = createdDepartment.HeadOfDepartment?.Name,
                    CreatedDate = createdDepartment.CreatedDate,
                    UpdatedDate = createdDepartment.UpdatedDate
                };

                return departmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while creating a new department. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<DepartmentDto> GetByIdAsync(int id)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(id);
                if (department is null || department.IsDeleted == true)
                {
                    throw new InvalidOperationException("Department not found");
                }

                DepartmentDto departmentDto = new()
                {
                    Id = department.Id,
                    Name = department.Name,
                    Description = department.Description,
                    HeadOfDepartmentId = department.HeadOfDepartmentId,
                    HeadOfDepartmentName = department.HeadOfDepartment?.Name,
                    CreatedDate = department.CreatedDate,
                    UpdatedDate = department.UpdatedDate
                };

                return departmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching department with id {id}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<DepartmentDto> UpdateAsync(UpdateDepartmentRequest request)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(request.Id);
                if (department is null || department.IsDeleted == true)
                {
                    throw new InvalidOperationException("Department not found");
                }

                var existingDepartment = await _departmentRepository.GetByNameAsync(request.Name);
                if (existingDepartment is not null && existingDepartment.Id != request.Id)
                {
                    throw new InvalidOperationException("Department name must be unique");
                }

                if (request.HeadOfDepartmentId.HasValue)
                {
                    var teacher = await _userRepository.GetByIdAsync(request.HeadOfDepartmentId.Value);
                    if (teacher is null)
                    {
                        throw new InvalidOperationException("Head of Department not found");
                    }

                    if (teacher.Role != UserRole.Teacher.ToString())
                    {
                        throw new InvalidOperationException("Only teachers can be assigned as Head of Department");
                    }
                }

                department.Name = request.Name;
                department.Description = request.Description;
                department.HeadOfDepartmentId = request.HeadOfDepartmentId;
                department.UpdatedDate = DateTime.UtcNow;

                await _departmentRepository.UpdateAsync(department);

                var updatedDepartment = await _departmentRepository.GetByIdAsync(department.Id);

                DepartmentDto departmentDto = new()
                {
                    Id = updatedDepartment!.Id,
                    Name = updatedDepartment.Name,
                    Description = updatedDepartment.Description,
                    HeadOfDepartmentId = updatedDepartment.HeadOfDepartmentId,
                    HeadOfDepartmentName = updatedDepartment.HeadOfDepartment?.Name,
                    CreatedDate = updatedDepartment.CreatedDate,
                    UpdatedDate = updatedDepartment.UpdatedDate
                };

                return departmentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while updating department with id {request.Id}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(id);
                if (department is null || department.IsDeleted == true)
                {
                    throw new InvalidOperationException("Department not found");
                }

                department.IsDeleted = true;
                department.UpdatedDate = DateTime.UtcNow;
                await _departmentRepository.UpdateAsync(department);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while deleting department with id {id}. {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<DepartmentDto>> GetAllAsync()
        {
            try
            {
                var departments = await _departmentRepository.GetAllAsync();

                List<DepartmentDto> departmentDtos = departments.Select(department => new DepartmentDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    Description = department.Description,
                    HeadOfDepartmentId = department.HeadOfDepartmentId,
                    HeadOfDepartmentName = department.HeadOfDepartment?.Name,
                    CreatedDate = department.CreatedDate,
                    UpdatedDate = department.UpdatedDate
                }).ToList();

                return departmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while fetching all departments. {ex.Message}", ex);
                throw;
            }
        }
    }
}
